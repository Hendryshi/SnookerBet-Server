﻿using SnookerBet.Core.Entities;
using SnookerBet.Core.Enumerations;
using SnookerBet.Core.JsonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Helper
{
	public class ConvertHelper
	{
		public static oQuizPredict ConvertToQuizPredict(Event evt, Gamer gamer, Quiz quiz)
		{
			bool isNewGamer = gamer == null;

			if(quiz.IdStatus > QuizStatus.OpenPredict && isNewGamer)
				return null;

			oQuizPredict quizPredict = new oQuizPredict()
			{
				IdEvent = evt.IdEvent,
				EventName = evt.Name,
				ReadOnly = (quiz.IdStatus == QuizStatus.MatchInProgress || quiz.IdStatus == QuizStatus.Done),
			};

			if(!isNewGamer)
				quizPredict.oGamer = ConvertToOGamer(gamer);

			foreach(EventRound er in evt.EventRounds)
			{
				oQuizRound quizRound = new oQuizRound(ConvertToORound(er));
				List<Match> matches = evt.EventMatches.FindAll(m => m.IdRound == er.IdRound);
				List<oMatch> oMatches = matches.Select(m => ConvertToOMatch(m)).ToList();

				foreach(oMatch om in oMatches)
				{
					Predict predict = isNewGamer ? null : gamer.predicts.Find(p => p.IdEvent == om.IdEvent && p.IdRound == om.IdRound && p.Number == om.Number);

					quizRound.oPredicts.Add(ConvertToOPredict(predict, om, quiz.IdStatus));
				}

				quizPredict.oQuizRounds.Add(quizRound);
			}

			return quizPredict;
		}

		public static oEvent ConvertToOEvent(Event evt, bool loadMatch = false)
		{
			oEvent oEvent = new oEvent()
			{
				IdEvent = evt.IdEvent,
				Name = evt.Name,
				StartDate = evt.StartDate,
				EndDate = evt.EndDate
			};

			if(loadMatch)
			{
				foreach(EventRound er in evt.EventRounds)
				{
					oEventRound oRound = new oEventRound(ConvertToORound(er));
					List<Match> matches = evt.EventMatches.FindAll(m => m.IdRound == er.IdRound);
					if(matches.Find(m => m.Player1Id != Constants.TBD || m.Player2Id != Constants.TBD) != null)
					{
						oRound.oMatches = matches.Select(m => ConvertToOMatch(m)).ToList();
						oEvent.oEventRounds.Add(oRound);
					}
				}
				oEvent.oEventRounds = oEvent.oEventRounds.OrderByDescending(o => o.IdRound).ToList();
			}
			return oEvent;
		}

		public static oRound ConvertToORound(EventRound er)
		{
			return new oRound()
			{
				IdRound = er.IdRound,
				RoundName = er.RoundName,
				Distance = er.Distance,
				Money = er.Money,
				Currency = er.Currency
			};
		}

		public static oMatch ConvertToOMatch(Match match)
		{
			return new oMatch()
			{
				IdEvent = match.IdEvent,
				IdRound = match.IdRound,
				Number = match.Number,
				Player1 = ConvertToOPlayer(match.Player1),
				Score1 = match.Score1.Value,
				Player2 = ConvertToOPlayer(match.Player2),
				Score2 = match.Score2.Value,
				WinnerId = match.WinnerId.Value,
				StMatch = match.StartDate == null ? MatchStatus.NotStart : (match.EndDate == null ? MatchStatus.Living : MatchStatus.Ended),
				ScheduledDate = match.ScheduledDate,
				note = match.note + " " + match.extendedNote
			};
		}

		public static oPlayer ConvertToOPlayer(Player player)
		{
			return new oPlayer()
			{
				IdPlayer = player.IdPlayer,
				Name = player.ChineseName ?? (player.LastName ?? player.FirstName),
				Photo = player.Photo,
				Rank = player.SeasonRank
			};
		}

		public static oPredict ConvertToOPredict(Predict predict, oMatch oMatch, QuizStatus quizStatus)
		{
			oPredict oPredict = null;

			if(predict == null || (quizStatus == QuizStatus.ReOpenPredict && oMatch.StMatch != MatchStatus.Ended))
			{
				oPredict = new oPredict(oMatch);

				//if(predict != null)
				//{
				//	oPredict.IdPredict = predict.IdPredict;
				//	oPredict.IdGamer = predict.IdGamer;
				//}
			}
			else
			{
				oPredict = ConvertToOPredict(predict);
				oPredict.ScheduledDate = oMatch.ScheduledDate;
				oPredict.StMatch = oMatch.StMatch;
				oPredict.note = oMatch.note;
			}

			if(oMatch.StMatch == MatchStatus.Ended)
				oPredict.predictStatus = PredictStatus.Ended;

			return oPredict;
		}

		public static oPredict ConvertToOPredict(Predict predict)
		{
			return new oPredict()
			{
				IdPredict = predict.IdPredict,
				//IdGamer = predict.IdGamer,
				IdRound = predict.IdRound,
				IdEvent = predict.IdEvent,
				Number = predict.Number,
				Player1 = ConvertToOPlayer(predict.Player1),
				Score1 = predict.Score1,
				Player2 = ConvertToOPlayer(predict.Player2),
				Score2 = predict.Score2,
				WinnerId = predict.WinnerId.Value,
				IsScoreCorrect = predict.ScoreCorrect,
				IsWinnerCorrect = predict.WinnerCorrect,
				predictStatus = predict.idStatus
			};
		}

		public static oGamer ConvertToOGamer(Gamer gm)
		{
			return new oGamer()
			{
				IdGamer = gm.IdGamer,
				IdEvent = gm.IdEvent,
				gamerName = gm.gamerName,
				wechatName = gm.wechatName
			};
		}
	}
}