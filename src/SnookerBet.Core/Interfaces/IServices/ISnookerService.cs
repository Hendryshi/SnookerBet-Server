using SnookerBet.Core.Entities;
using SnookerBet.Core.JsonObjects;
using System;
using System.Collections.Generic;

namespace SnookerBet.Core.Interfaces
{
	public interface ISnookerService
	{
		List<Match> GetEndedMatchInDay(DateTime? dtStamp = null);
		Event GetEventById(int idEvent, bool loadMatch = false);
		oEvent GetEventInfoWithMatches(int idEvent);
		Match GetMatchInfo(int idEvent, int idRound, int idNumber);
		List<oMatch> GetOnGoingMatch();
		EventRound GetRoundInfo(int idEvent, int idRound);
		Event UpdateEventInfo(int idEvent, bool isInit = false);
		void UpdateEventsInSeason(int season);
		void UpdatePlayerById(int idPlayer);
		void UpdatePlayersInEvent(int idEvent);
		void UpdatePlayersInSeason(int season);
	}
}