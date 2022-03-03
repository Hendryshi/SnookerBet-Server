using SnookerBet.Core.Entities;

namespace SnookerBet.Core.Interfaces
{
	public interface IGamerRepo
	{
		Gamer FindByEvent(int idEvent, string wechatName, bool loadPredict = true);
		Gamer Save(Gamer gamer);
	}
}