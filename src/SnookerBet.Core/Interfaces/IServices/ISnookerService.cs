using SnookerBet.Core.Entities;

namespace SnookerBet.Core.Interfaces
{
	public interface ISnookerService
	{
		oEvent GetEventInfoWithMatches(int idEvent);
		void UpdateEventInfo(int idEvent);
		void UpdateEventsInSeason(int season);
		void UpdatePlayerById(int idPlayer);
		void UpdatePlayersInSeason(int season);
	}
}