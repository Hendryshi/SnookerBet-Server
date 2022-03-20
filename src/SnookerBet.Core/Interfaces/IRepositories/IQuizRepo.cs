using SnookerBet.Core.Entities;
using SnookerBet.Core.Enumerations;
using SnookerBet.Core.JsonObjects;
using System.Collections.Generic;

namespace SnookerBet.Core.Interfaces
{
	public interface IQuizRepo
	{
		Quiz FindByEvent(int idEvent);
		List<Quiz> FindByStatus(List<QuizStatus> quizStatuses);
		List<oPredictByDay> GetPredictPointByDay(int idEvent, int idGamer);
		List<oPredictStat> GetPredictSummary(int idEvent, int idGamer = 0);
		Quiz Save(Quiz quiz);
		QuizSummary SaveSummary(QuizSummary summary);
		List<QuizSummary> GetQuizSummary(int idEvent);
	}
}