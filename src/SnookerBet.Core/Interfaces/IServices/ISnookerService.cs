namespace SnookerBet.Core.Interfaces
{
	public interface ISnookerService
	{
		void UpdateEventInfo(int idEvent);
		void UpdateEventsInSeason(int season);
		void UpdatePlayersInSeason(int season);
	}
}