using SnookerBet.Core.Entities;
using SnookerBet.Core.JsonObjects;

namespace SnookerBet.Core.Interfaces
{
	public interface IQuizService
	{
		Quiz CreateQuiz(int idEvent);
		oQuizPredict GetQuizPredict(int idEvent, string wechatId);
	}
}