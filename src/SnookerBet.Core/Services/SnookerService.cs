﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SnookerBet.Core.Interfaces;
using SnookerBet.Core.Entities;
using SnookerBet.Core.JsonObjects;
using SnookerBet.Core.Helper;
using SnookerBet.Core.Extensions;

namespace SnookerBet.Core.Services
{
	public class SnookerService : ISnookerService
	{
		private readonly IEventRepo _eventRepo;
		private readonly IMatchRepo _matchRepo;
		private readonly IEventRoundRepo _roundRepo;
		private readonly IPlayerRepo _playerRepo;
		private readonly IExternalDataService _externalDataService;
		private readonly IAppLogger<SnookerService> _logger;

		public SnookerService(IAppLogger<SnookerService> logger, IEventRepo eventRepo,
			IEventRoundRepo eventRoundRepo,
			IMatchRepo matchRepo,
			IPlayerRepo playerRepo,
			IExternalDataService externalDataService)
		{
			_eventRepo = eventRepo;
			_roundRepo = eventRoundRepo;
			_matchRepo = matchRepo;
			_playerRepo = playerRepo;
			_externalDataService = externalDataService;
			_logger = logger;
		}

		public Event GetEventById(int idEvent, bool loadMatch = false)
		{
			return _eventRepo.FindById(idEvent, loadMatch);
		}

		public Match GetMatchInfo(int idEvent, int idRound, int idNumber)
		{
			return _matchRepo.FindById(idEvent, idRound, idNumber);
		}

		public EventRound GetRoundInfo(int idEvent, int idRound)
		{
			return _roundRepo.FindById(idEvent, idRound);
		}

		public oEvent GetEventInfoWithMatches(int idEvent)
		{
			Event evt = _eventRepo.FindById(idEvent, true);
			if(evt == null)
				throw new ApplicationException($"Cannot find event [id={idEvent}] from db");

			return ConvertHelper.ConvertToOEvent(evt, true);
		}

		public List<oMatch> GetOnGoingMatch()
		{
			List<oMatch> oMatches = new List<oMatch>();
			List<Match> matches = _matchRepo.GetOnGoingMatches();

			foreach(Match m in matches)
			{
				EventRound r = GetRoundInfo(m.IdEvent, m.IdRound);
				oMatch om = ConvertHelper.ConvertToOMatch(m);
				if(r != null)
					om.RoundName = r.RoundName.Translate("Round");

				oMatches.Add(om);
			}

			return oMatches;
		}

		public List<Match> GetEndedMatchInDay(int idEvent, DateTime? dtStamp = null)
		{
			if(dtStamp == null) dtStamp = DateTime.Now;
			return _matchRepo.GetEndedMatchInDay(idEvent, dtStamp.Value);
		}

		public void UpdateEventsInSeason(int season)
		{
			_logger?.LogInformation("Start update all events info in season {0} ", season);
			List<Event> events = _externalDataService.GetEventsInSeason(season)?.FindAll(e => e.TyEvent == "Ranking");

			if(events != null && events.Count > 0)
			{
				_eventRepo.SaveList(events);
				_logger?.LogInformation("{0} events in season {1} have been updated", events.Count, season);
			}
			else
			{
				_logger?.LogWarning("Cannot find any ranking events in season {0}", season);
			}
		}

		public void UpdatePlayersInSeason(int season)
		{
			_logger?.LogInformation("Start update all players info in season {0} ", season);
			List<Player> players = _externalDataService.GetPlayersInSeason(season);

			if(players != null && players.Count > 0)
			{
				List<Rank> ranks = _externalDataService.GetSeasonRankInSeason(season);

				if(ranks != null && ranks.Count > 0)
					players.ForEach(p => p.SeasonRank = ranks.Find(r => r.PlayerId == p.IdPlayer)?.Position);
				else
					_logger?.LogWarning("Cannot find any ranking info in season {0}", season);

				_playerRepo.SaveList(players);
				_logger?.LogInformation("{0} players in season {1} have been updated", players.Count, season);
			}
			else
			{
				_logger?.LogWarning("Cannot find any players in season {0}", season);
			}
		}

		public void UpdatePlayersInEvent(int idEvent)
		{
			_logger?.LogInformation("Start update all players info in event[id={idEvent}] ", idEvent);
			List<Player> players = _externalDataService.GetPlayersInEvent(idEvent);

			if(players != null && players.Count > 0)
			{
				List<Rank> ranks = _externalDataService.GetSeasonRankInSeason(0);

				if(ranks != null && ranks.Count > 0)
					players.ForEach(p => p.SeasonRank = ranks.Find(r => r.PlayerId == p.IdPlayer)?.Position);
				else
					throw new ApplicationException("Cannot find any ranking info in current season");

				_playerRepo.SaveList(players);
				_logger?.LogInformation("{0} players in current season have been updated", players.Count);
			}
			else
			{
				throw new ApplicationException($"Cannot find any players in event[id={idEvent}]");
			}
		}

		public void UpdatePlayerById(int idPlayer)
		{
			Player pl = _externalDataService.GetPlayer(idPlayer);
			if(pl == null)
				throw new ApplicationException($"Cannot find player[id={idPlayer}] from external api");

			_playerRepo.Save(pl);
		}

		public Event UpdateEventInfo(int idEvent, bool isInit = false)
		{
			_logger?.LogInformation("Start update event and details info for event [id={0}] ", idEvent);
			Event evt = _externalDataService.GetEvent(idEvent);
			if(evt == null)
				throw new ApplicationException($"Cannot find event [id={idEvent}] from external api");

			List<EventRound> eventRounds = _externalDataService.GetRoundsInEvent(idEvent);
			if(eventRounds == null || eventRounds.Count == 0)
				throw new ApplicationException($"Cannot find any round info for event [id={idEvent}] from external api");

			evt.EventRounds = eventRounds.FindAll(r => r.IdEvent == idEvent && r.Distance > 0);

			List<Match> matches = _externalDataService.GetMatchesInEvent(idEvent);
			if(matches == null || matches.Count == 0)
				throw new ApplicationException($"Cannot find any match for event [id={idEvent}] from external api");

			if(isInit)
				UpdatePlayersInEvent(idEvent);

			foreach(Match m in matches.FindAll(m => m.IdEvent == idEvent))
			{
				Match mInDb = _matchRepo.FindById(m.IdEvent, m.IdRound, m.Number);
				if(mInDb != null)
				{
					mInDb.Player1Id = m.Player1Id;
					mInDb.Score1 = m.Score1;
					mInDb.Player2Id = m.Player2Id;
					mInDb.Score2 = m.Score2;
					mInDb.WinnerId = m.WinnerId;
					mInDb.Unfinished = m.Unfinished;
					mInDb.OnBreak = m.OnBreak;
					mInDb.InitDate = m.InitDate;
					mInDb.ModDate = m.ModDate;
					mInDb.StartDate = m.StartDate;
					mInDb.EndDate = m.EndDate;
					mInDb.ScheduledDate = m.ScheduledDate;
					mInDb.note = m.note;
					mInDb.extendedNote = m.extendedNote;
				}
				else
					mInDb = m;

				evt.EventMatches.Add(mInDb);
			}

			_logger?.LogInformation("End update event and details info for event [id={0}] ", idEvent);
			return _eventRepo.Save(evt, false);
		}

	}
}
