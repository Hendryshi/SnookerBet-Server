using SnookerBet.Core.Entities;
using System.Collections.Generic;

namespace SnookerBet.Core.Interfaces
{
	public interface IGamerRepo
	{
		Gamer FindByEventAndName(int idEvent, string wechatName, bool loadPredict = true);
		
		Gamer FindById(int idGamer);
		List<Gamer> LoadAllByEvent(int idEvent, bool loadPredict = true);
		Gamer Save(Gamer gamer, bool updatePredict = false);
		List<Gamer> SaveList(List<Gamer> gamers, bool updatePredict = false);
	}
}