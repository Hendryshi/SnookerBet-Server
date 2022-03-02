using SnookerBet.Core.Entities;
using SnookerBet.Core.JsonObjects;

namespace SnookerBet.Core.Interfaces
{
	public interface ISnookerService
	{
		oEvent GetEventInfoWithMatches(int idEvent);
		Event UpdateEventInfo(int idEvent);
		void UpdateEventsInSeason(int season);
		void UpdatePlayerById(int idPlayer);
		void UpdatePlayersInSeason(int season);
	}
}