using System;
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

namespace SnookerBet.Core.Services
{
	public class QuizService : IQuizService
	{
		private readonly ISnookerService _snookerService;
		private readonly IQuizRepo  _quizRepo;
		private readonly IGamerRepo _gamerRepo;
		private readonly IPredictRepo _predictRepo;
		private readonly IAppLogger<QuizService> _logger;

		public QuizService(IAppLogger<QuizService> logger,
			ISnookerService snookerService,
			IQuizRepo quizRepo,
			IGamerRepo gamerRepo, 
			IPredictRepo predictRepo)
		{
			_snookerService = snookerService;
			_quizRepo = quizRepo;
			_gamerRepo = gamerRepo;
			_predictRepo = predictRepo;
			_logger = logger;
		}

		public List<oQuiz> GetAvailableQuiz()
		{
			List<oQuiz> oQuizzes = new List<oQuiz>();
			List<Quiz> quizzes = _quizRepo.FindByStatus(new List<QuizStatus> { QuizStatus.OpenPredict, QuizStatus.InProgress, QuizStatus.ReOpenPredict, QuizStatus.InSecondProgress, QuizStatus.Done });
			foreach(Quiz quiz in quizzes)
			{
				Event evt = _snookerService.GetEventById(quiz.IdEvent);
				if(evt != null)
					oQuizzes.Add(ConvertHelper.ConverToOQuiz(evt, quiz));
			}
			return oQuizzes.OrderByDescending(q => q.IdQuiz).ToList();
		}

		public oQuizPredict GetQuizPredict(int idEvent, string wechatId)
		{
			Quiz quiz = _quizRepo.FindByEvent(idEvent);
			if(quiz == null)
				throw new ApplicationException($"Cannot find quiz for Event[id={idEvent}] in DB");

			Event evt = _snookerService.GetEventById(idEvent, true);
			Gamer gamer = _gamerRepo.FindByEventAndName(idEvent, wechatId);

			return ConvertHelper.ConvertToQuizPredict(evt, gamer, quiz);
		}

		public oQuizMatch GetQuizMatch(int idEvent, int idRound, int idNumber)
		{
			Match match = _snookerService.GetMatchInfo(idEvent, idRound, idNumber);
			if(match == null)
				throw new ApplicationException($"Cannot find match[idEvent={idEvent} - idRound={idRound} - Number={idNumber}] in DB");

			List<Predict> predicts = _predictRepo.FindByMatch(idEvent, idRound, idNumber);
			List<oPredict> oPredicts = new List<oPredict>();
			foreach(Predict p in predicts)
			{
				if((p.Player1Id == match.Player1Id || p.Player2Id == match.Player2Id) && !(p.idStatus == PredictStatus.Ended && p.Point == null))
				{
					oPredict op = ConvertHelper.ConvertToOPredict(p);
					op.GamerName = _gamerRepo.FindById(p.IdGamer).GamerName;
					oPredicts.Add(op);
				}
			}
			return new oQuizMatch() { OMatch = ConvertHelper.ConvertToOMatch(match), oPredicts = oPredicts };
		}

		public Quiz GetCurrentQuiz()
		{
			List<Quiz> quizzes = _quizRepo.FindByStatus(new List<QuizStatus> { QuizStatus.OpenPredict, QuizStatus.InProgress, QuizStatus.ReOpenPredict, QuizStatus.InSecondProgress, QuizStatus.Done });
			if(quizzes.Count == 0)
				return null;

			return quizzes.OrderByDescending(q => q.IdQuiz).First();
		}

		public List<oPredictStat> GetPredictSummary(int? idEvent = null)
		{
			List<oPredictStat> oPredictStats = new List<oPredictStat>();
			

			if(idEvent == null) idEvent = GetCurrentQuiz()?.IdEvent;
			
			if(idEvent != null)
				oPredictStats = _quizRepo.GetPredictSummary(idEvent.Value);
			
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

			Event evt = _snookerService.UpdateEventInfo(idEvent);
			_snookerService.UpdatePlayersInEvent(idEvent);

			int idRoundMin = evt.EventRounds.First().IdRound;

			List<Match> firstRoundMatches = evt.EventMatches.FindAll(m => m.IdRound == idRoundMin);
			if(firstRoundMatches.Find(m => m.Player1Id == Constants.TBD || m.Player2Id == Constants.TBD) != null)
				throw new ApplicationException($"Cannot create the quiz for Event[id={idEvent}]. Still has TBD player in first round");

			return _quizRepo.Save(new Quiz() { IdEvent = idEvent, DtStart = DateTime.Today });
		}

		
		public void UpdateQuizPredict(oQuizPredict quizPredict)
		{
			int idEvent = quizPredict.IdEvent;
			oGamer oGamer = quizPredict.oGamer;
			Gamer gamer = _gamerRepo.FindByEventAndName(idEvent, oGamer.wechatName, false);
			if(gamer == null)
				gamer = new Gamer() { IdEvent = idEvent, WechatName = oGamer.wechatName, GamerName = oGamer.gamerName };

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
	}
}
