using SnookerBet.Core.Entities;
using SnookerBet.Core.JsonObjects;

namespace SnookerBet.Core.Interfaces
{
	public interface ISnookerService
	{
		Event GetEventById(int idEvent, bool loadMatch = false);
		oEvent GetEventInfoWithMatches(int idEvent);
		Match GetMatchInfo(int idEvent, int idRound, int idNumber);
		Event UpdateEventInfo(int idEvent);
		void UpdateEventsInSeason(int season);
		void UpdatePlayerById(int idPlayer);
		void UpdatePlayersInEvent(int idEvent);
		void UpdatePlayersInSeason(int season);
	}
}