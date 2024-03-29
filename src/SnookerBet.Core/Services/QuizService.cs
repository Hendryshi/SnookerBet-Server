﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SnookerBet.Core.Interfaces;
using SnookerBet.Core.Entities;
using SnookerBet.Core.Enumerations;
using SnookerBet.Core.JsonObjects;
using SnookerBet.Core.Helper;
using System.Transactions;
using SnookerBet.Core.Settings;
using Microsoft.Extensions.Options;
using SnookerBet.Core.Extensions;

namespace SnookerBet.Core.Services
{
	public class QuizService : IQuizService
	{
		private readonly ISnookerService _snookerService;
		private readonly IQuizRepo  _quizRepo;
		private readonly IGamerRepo _gamerRepo;
		private readonly IPredictRepo _predictRepo;
		private readonly IWechatService _wechatService;
		private readonly IAppLogger<QuizService> _logger;
		private readonly QuizSettings _quizSettings;

		public QuizService(IAppLogger<QuizService> logger,
			ISnookerService snookerService,
			IQuizRepo quizRepo,
			IGamerRepo gamerRepo, 
			IPredictRepo predictRepo,
			IWechatService wechatService,
			IOptionsSnapshot<QuizSettings> quizSettings)
		{
			_snookerService = snookerService;
			_quizRepo = quizRepo;
			_gamerRepo = gamerRepo;
			_predictRepo = predictRepo;
			_logger = logger;
			_wechatService = wechatService;
			_quizSettings = quizSettings.Value;
		}

		public Quiz Save(Quiz quiz)
		{
			return _quizRepo.Save(quiz);
		}

		public List<oQuiz> GetAvailableQuiz()
		{
			List<oQuiz> oQuizzes = new List<oQuiz>();
			List<Quiz> quizzes = _quizRepo.FindByStatus(new List<QuizStatus> { QuizStatus.OpenPredict, QuizStatus.InProgress, QuizStatus.InDoubleProgress, QuizStatus.Done });
			foreach(Quiz quiz in quizzes)
			{
				Event evt = _snookerService.GetEventById(quiz.IdEvent);
				if(evt != null)
					oQuizzes.Add(ConvertHelper.ConverToOQuiz(evt, quiz));
			}

			if(oQuizzes.Count > 0)
				oQuizzes = oQuizzes.OrderByDescending(q => q.IdQuiz).Take(1).ToList();

			return oQuizzes;
		}

		public oQuizPredict GetQuizPredict(int idEvent, string wechatId, bool isReEdit = false)
		{
			Quiz quiz = _quizRepo.FindByEvent(idEvent);
			if(quiz == null)
				throw new ApplicationException($"Cannot find quiz for Event[id={idEvent}] in DB");

			Event evt = _snookerService.GetEventById(idEvent, true);
			Gamer gamer = _gamerRepo.FindByEventAndName(idEvent, wechatId);

			return ConvertHelper.ConvertToQuizPredict(evt, gamer, quiz, isReEdit);
		}

		public oQuizMatch GetQuizMatch(int idEvent, int idRound, int idNumber)
		{
			Match match = _snookerService.GetMatchInfo(idEvent, idRound, idNumber);
			if(match == null)
				throw new ApplicationException($"Cannot find match[idEvent={idEvent} - idRound={idRound} - Number={idNumber}] in DB");

			EventRound r = _snookerService.GetRoundInfo(match.IdEvent, match.IdRound);
			List<Predict> predicts = _predictRepo.FindByMatch(idEvent, idRound, idNumber);
			List<oPredict> oPredicts = new List<oPredict>();
			foreach(Predict p in predicts)
			{
				if((match.Player1Id == Constants.TBD || match.Player2Id == Constants.TBD || p.Player1Id == match.Player1Id || p.Player2Id == match.Player2Id) && !(p.idStatus == PredictStatus.Ended && p.Point == null))
				{
					oPredict op = ConvertHelper.ConvertToOPredict(p);
					op.GamerName = _gamerRepo.FindById(p.IdGamer).GamerName;
					oPredicts.Add(op);
				}
			}
			oMatch om = ConvertHelper.ConvertToOMatch(match);
			if(r != null)
				om.RoundName = r.RoundName.Translate("Round");

			return new oQuizMatch() { OMatch = om, oPredicts = oPredicts };
		}

		public Quiz GetCurrentQuiz()
		{
			List<Quiz> quizzes = _quizRepo.FindByStatus(new List<QuizStatus> { QuizStatus.OpenPredict, QuizStatus.InProgress, QuizStatus.InDoubleProgress, QuizStatus.Done });
			if(quizzes.Count == 0)
				return null;

			return quizzes.OrderByDescending(q => q.IdQuiz).First();
		}

		public List<oPredictStat> GetPredictSummary(int? idEvent = null)
		{
			List<oPredictStat> oPredictStats = new List<oPredictStat>();
			

			if(idEvent == null) idEvent = GetCurrentQuiz()?.IdEvent;
			
			if(idEvent != null)
			{
				List<Gamer> gamers = _gamerRepo.LoadAllByEvent(idEvent.Value, false);
				foreach(Gamer gamer in gamers)
				{
					oPredictStat predictStat = _quizRepo.GetPredictSummary(idEvent.Value, gamer.IdGamer).FirstOrDefault();

					if(predictStat == null)
						predictStat = new oPredictStat() { IdEvent = idEvent.Value, IdGamer = gamer.IdGamer, GamerName = gamer.GamerName };

					oPredictStats.Add(predictStat);
				}
			}

			return oPredictStats.OrderByDescending(p => p.TotalPoint).ToList();
		}

		public List<oPredictGamerTrend> GetPredictTrending(int? idEvent = null)
		{
			List<oPredictGamerTrend> oPredictGamerTrends = new List<oPredictGamerTrend>();

			if(idEvent == null) idEvent = GetCurrentQuiz()?.IdEvent;

			if(idEvent != null)
			{
				List<Gamer> gamers = _gamerRepo.LoadAllByEvent(idEvent.Value, false);

				foreach(Gamer gamer in gamers)
				{
					List<oPredictByDay> oPredictByDays = _quizRepo.GetPredictPointByDay(idEvent.Value, gamer.IdGamer);
					oPredictGamerTrends.Add(new oPredictGamerTrend() { IdGamer = gamer.IdGamer, IdEvent = idEvent.Value, GamerName = gamer.GamerName, oPredictByDays = oPredictByDays });
				}
			}

			return oPredictGamerTrends;
		}

		public Quiz CreateQuiz(int idEvent)
		{
			if(_quizRepo.FindByEvent(idEvent) != null)
				throw new ApplicationException($"The quiz for Event[id={idEvent}] has already created");

			Quiz lastQuiz = GetCurrentQuiz();
			if(lastQuiz != null && lastQuiz.IdStatus != QuizStatus.Done)
				throw new ApplicationException($"Another quiz has not been finished");

			using(var trans = new TransactionScope())
			{
				Event evt = _snookerService.UpdateEventInfo(idEvent, true);
				
				int idRoundMin = evt.EventRounds.First().IdRound;

				List<Match> firstRoundMatches = evt.EventMatches.FindAll(m => m.IdRound == idRoundMin);
				if(firstRoundMatches.Find(m => m.Player1Id == Constants.TBD || m.Player2Id == Constants.TBD) != null)
					throw new ApplicationException($"Cannot create the quiz for Event[id={idEvent}]. Still has TBD player in first round");

				Quiz q = _quizRepo.Save(new Quiz() { IdEvent = idEvent, DtStart = DateTime.Today });
				trans.Complete();

				return q;
			}
		}

		public List<QuizSummary> GetQuizSummary(int? idEvent = null)
		{
			List<QuizSummary> quizSummaries = new List<QuizSummary>();
			if(idEvent == null) idEvent = GetCurrentQuiz()?.IdEvent;

			if(idEvent != null)
				quizSummaries = _quizRepo.GetQuizSummary(idEvent.Value);

			return quizSummaries;
		}
		
		public void UpdateQuizPredict(oQuizPredict quizPredict)
		{
			int idEvent = quizPredict.IdEvent;
			oGamer oGamer = quizPredict.oGamer;
			Gamer gamer = _gamerRepo.FindByEventAndName(idEvent, oGamer.WechatName, false);
			if(gamer == null)
			{
				gamer = new Gamer() { IdEvent = idEvent, WechatName = oGamer.WechatName, GamerName = oGamer.GamerName };
				string openId = _wechatService.GetUserOpenId(oGamer.WechatCode);
				if(!string.IsNullOrEmpty(openId))
					gamer.WechatCode = openId;
			}
			else
				gamer.NbEditPredict++;

			foreach(oQuizRound qr in quizPredict.oQuizRounds)
			{
				foreach(oPredict op in qr.oPredicts)
				{
					Predict p = _predictRepo.FindByMatchAndGamer(gamer.IdGamer, op.IdEvent, op.IdRound, op.Number);
					gamer.predicts.Add(ConvertHelper.ConvertFromOPredict(p, op));
				}
			}

			_gamerRepo.Save(gamer, true);
		}

		public void SendNotification()
		{
			int? idEvent = GetCurrentQuiz()?.IdEvent;
			if(idEvent != null)
			{
				string token = _wechatService.GetAccessToken();
				if(string.IsNullOrEmpty(token))
					throw new ApplicationException($"Cannot get wechat token");

				List <Gamer> gamers = _gamerRepo.LoadAllByEvent(idEvent.Value, false).OrderByDescending(g => g.TotalScore).ToList();
				int index = 0;
				foreach(Gamer gamer in gamers)
				{
					index++;
					if(gamer.WechatCode == null)
					{
						_logger.LogWarning($"Gamer {gamer.WechatName} have not wechat code");
						continue;
					}

					if(gamer.NbEditPredict > 0)
					{
						_logger.LogWarning($"Gamer {gamer.WechatName} has already changed his predict");
						continue;
					}

					_logger.LogInformation($"Send notification for gamer {gamer.WechatName}");

					WechatNotif wn = new WechatNotif(){ touser = gamer.WechatCode };
					string rank = $"您目前的排名为第{index}名";
					var data = new
					{
						thing2 = new { value = "您可以开始第二次竞猜预测" },
						thing3 = new { value = rank },
					};
					wn.data = data;

					_wechatService.SendNotification(token, wn);
				}
			}
		}

		public void CalculateGamerScore(int idEvent = 0, DateTime? dtStamp = null)

		{
			if(dtStamp == null) dtStamp = DateTime.Now;

			_logger.LogInformation("************************************************************");
			_logger.LogInformation("************************************************************");
			_logger.LogInformation($"Start calcuate the score for ended match in day [{dtStamp} - IdEvent: {idEvent}]");
			_logger.LogInformation("************************************************************");
			_logger.LogInformation("************************************************************");

			List<Match> matches = _snookerService.GetEndedMatchInDay(idEvent, dtStamp);
			if(matches.Count == 0)
			{
				_logger.LogInformation($"No match has been found ended in day");
				return;
			}

			_logger.LogInformation($"Found {matches.Count} matches ended in day");
			int nbTreated = 0;

			Dictionary<int, int> gamerScoreDic = new Dictionary<int, int>();
			QuizSummary summary = new QuizSummary() { DtResult = dtStamp.Value };

			List<Predict> allPredicts = new List<Predict>();

			foreach(Match match in matches)
			{
				List<Predict> predicts = _predictRepo.FindByMatch(match.IdEvent, match.IdRound, match.Number).FindAll(p => p.idStatus != PredictStatus.Ended);
				Quiz quiz = _quizRepo.FindByEvent(match.IdEvent);
				summary.IdEvent = match.IdEvent;
				
				if(predicts.Count > 0)
				{
					
					List<Predict> predictWinners = predicts.FindAll(p => p.WinnerId == match.WinnerId).ToList();
					List<Predict> predictScores = predictWinners.FindAll(p => p.Player1Id == match.Player1Id && p.Player2Id == match.Player2Id && p.Score1 == match.Score1 && p.Score2 == match.Score2).ToList();

					int totalCount = predictWinners.Count + predictScores.Count * 2;
					decimal winnerPoint = Math.Round((predictWinners.Count > 0 ? ((decimal)100 / totalCount) : 0), 2);
					decimal scorePoint = winnerPoint * 2;
					
					_logger?.LogInformation($"Match[idEvent={match.IdEvent}-idRound={match.IdRound}-Number={match.Number}]: Score: {match.Score1}-{match.Score2}, NbWinnerCorrect: {predictWinners.Count}, NbScoreCorrect: {predictScores.Count}, WinnerPoint: {winnerPoint}, ScorePoint: {scorePoint}");
					string player1Name = ConvertHelper.ConvertToOPlayer(match.Player1).Name;
					string player2Name = ConvertHelper.ConvertToOPlayer(match.Player2).Name;

					summary.DescMatchSummary += $"{player1Name} {match.Score1}:{match.Score2} {player2Name},   ";

					foreach(Predict p in predicts)
					{
						int coef = Math.Abs(match.Score1.Value - match.Score2.Value) == 1 ? 2 : 1;
						Gamer gamer = _gamerRepo.FindById(p.IdGamer);
						if(quiz.IdStatus == QuizStatus.InDoubleProgress && gamer.NbEditPredict == 0)
							coef *= 2;

						decimal point = 0;

						if(p.WinnerId == match.WinnerId)
						{
							p.WinnerCorrect = true;
							point += winnerPoint * coef;

							if(p.IdRound == 13)
								point += _quizSettings.QuarterFinalScore;
							else if(p.IdRound == 14)
								point += _quizSettings.SemiFinalScore;
							else if(p.IdRound == 15)
								point += _quizSettings.FinalScore;
						}

						if(p.Player1Id == match.Player1Id && p.Player2Id == match.Player2Id && p.Score1 == match.Score1 && p.Score2 == match.Score2)
						{
							p.ScoreCorrect = true;
							point += scorePoint * coef;
						}

						int finalPoint = (int)Math.Round(point);
						p.Point = finalPoint;
						p.DtResult = dtStamp;
						p.idStatus = PredictStatus.Ended;

						if(gamerScoreDic.ContainsKey(gamer.IdGamer))
							gamerScoreDic[gamer.IdGamer] += finalPoint;
						else
							gamerScoreDic.Add(gamer.IdGamer, finalPoint);

						_logger.LogInformation($"Predict[id={p.IdPredict}-IdGamer={p.IdGamer}]:Score: {p.Score1}-{p.Score2}, IsWinnerCorrect={p.WinnerCorrect}, IsScoreCorrect={p.ScoreCorrect}, Coef={coef}, Point={finalPoint}");
					}

					nbTreated += 1;
					allPredicts.AddRange(predicts);
				}
			}

			if(!string.IsNullOrEmpty(summary.DescMatchSummary))
			{
				summary.DescMatchSummary = summary.DescMatchSummary.TrimEnd().TrimEnd(',');
				_logger.LogInformation($"DescMatchSummary: {summary.DescMatchSummary}");
			}


			_logger.LogInformation($"{nbTreated} matches have been treated. Now save all the predicts");
			
			if(allPredicts.Count > 0)
				_predictRepo.SaveList(allPredicts);

			if(gamerScoreDic.Count > 0)
			{
				_logger.LogInformation("Saving gamers");
				List<Gamer> gamers = new List<Gamer>();
				foreach(KeyValuePair<int, int> gamerScore in gamerScoreDic)
				{
					Gamer gamer = _gamerRepo.FindById(gamerScore.Key);

					summary.DescPointSummary += $"{gamer.GamerName}: {gamerScore.Value},   ";

					gamer.TotalScore += gamerScore.Value;
					gamers.Add(gamer);
				}

				if(!string.IsNullOrEmpty(summary.DescPointSummary))
				{
					summary.DescPointSummary = summary.DescPointSummary.TrimEnd().TrimEnd(',');
					_logger.LogInformation($"DescPointSummary: {summary.DescPointSummary}");
				}

				if(gamers.Count > 0)
					_gamerRepo.SaveList(gamers);

				_quizRepo.SaveSummary(summary);
			}

			_logger.LogInformation("************************************************************");
			_logger.LogInformation("************************************************************");
			_logger.LogInformation($"End calcuate the score for ended match in day - {dtStamp}");
			_logger.LogInformation("************************************************************");
			_logger.LogInformation("************************************************************");
		}
	}
}
