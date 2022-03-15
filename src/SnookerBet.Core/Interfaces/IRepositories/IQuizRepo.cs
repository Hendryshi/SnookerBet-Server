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
		QuizSummary GetLastSummary(int idEvent);
		List<oPredictByDay> GetPredictPointByDay(int idEvent, int idGamer);
		List<oPredictStat> GetPredictSummary(int idEvent);
		Quiz Save(Quiz quiz);
		QuizSummary SaveSummary(QuizSummary summary);
	}
}