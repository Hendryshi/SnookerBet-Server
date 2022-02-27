using SnookerBet.Core.Entities;
using System.Collections.Generic;

namespace SnookerBet.Core.Interfaces
{
	public interface IExternalDataService
	{
		Event GetEvent(int idEvent);
		List<Event> GetEventsInSeason(int season);
		List<Match> GetMatchesInEvent(int idEvent);
		List<Player> GetPlayersInSeason(int season);
		List<EventRound> GetRoundsInEvent(int idEvent);
		List<Rank> GetSeasonRankInSeason(int season = 0);
		Player GetPlayer(int idPlayer);
	}
}