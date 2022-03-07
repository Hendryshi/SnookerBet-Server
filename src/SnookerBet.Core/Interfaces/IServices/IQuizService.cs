using SnookerBet.Core.Entities;
using SnookerBet.Core.JsonObjects;
using System;
using System.Collections.Generic;

namespace SnookerBet.Core.Interfaces
{
	public interface IQuizService
	{
		Quiz CreateQuiz(int idEvent);
		List<oQuiz> GetAvailableQuiz();
		oQuizMatch GetQuizMatch(int idEvent, int idRound, int idNumber);
		oQuizPredict GetQuizPredict(int idEvent, string wechatId);
		void UpdateQuizPredict(oQuizPredict quizPredict);
		List<oPredictGamerTrend> GetPredictTrending(int? idEvent = null);
		List<oPredictStat> GetPredictSummary(int? idEvent = null);
		void CalculateGamerScore(DateTime? dtStamp = null);
	}
}