using SnookerBet.Core.Entities;

namespace SnookerBet.Core.Interfaces
{
	public interface IQuizRepo
	{
		Quiz FindByEvent(int idEvent);
		Quiz Save(Quiz quiz);
	}
}