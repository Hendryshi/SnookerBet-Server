using SnookerBet.Core.Entities;
using SnookerBet.Core.JsonObjects;
using System;
using System.Collections.Generic;

namespace SnookerBet.Core.Interfaces
{
	public interface IQuizService
	{
		Quiz CreateQuiz(int idEvent);
		oQuizMatch GetQuizMatch(int idEvent, int idRound, int idNumber);
		void UpdateQuizPredict(oQuizPredict quizPredict);
		List<oPredictGamerTrend> GetPredictTrending(int? idEvent = null);
		List<oPredictStat> GetPredictSummary(int? idEvent = null);
		oQuizPredict GetQuizPredict(int idEvent, string wechatId, bool isReEdit = false);
		List<QuizSummary> GetQuizSummary(int? idEvent = null);
		Quiz GetCurrentQuiz();
		List<oQuiz> GetAvailableQuiz();
		void CalculateGamerScore(int idEvent = 0, DateTime? dtStamp = null);
		Quiz Save(Quiz quiz);
	}
}