using System;
using Xunit;
using Moq;
using UnitTests.Builders;
using Microsoft.Extensions.Options;
using SnookerBet.Core.Services;
using SnookerBet.Core.Settings;
using SnookerBet.Core.Entities;
using SnookerBet.Infrastructure.Logging;
using System.Collections.Generic;
using Xunit.Abstractions;
using System.Linq;

namespace UnitTests
{
	public class ExternalDataServiceTests
	{
		private readonly ITestOutputHelper _output;
		private readonly ExternalDataService _externalDataService;

		public ExternalDataServiceTests(ITestOutputHelper output)
		{
			_output = output;
			_externalDataService = new ExternalDataServiceBuilder().Build();
		}

		[Fact]
		public void TestGetEvent()	
		{
			int idEvent = 0;
			Event evt = _externalDataService.GetEvent(idEvent);
			_output.WriteLine(evt.ToString());
		}

		[Fact]
		public void TestGetEventsInSeason()
		{
			int idSeason = 2021;
			List<Event> events = _externalDataService.GetEventsInSeason(idSeason);
			_output.WriteLine(events.Count.ToString());
		}

		[Fact]
		public void TestGetPlayersInSeason()
		{
			int idSeason = 2021;
			List<Player> players = _externalDataService.GetPlayersInSeason();
			_output.WriteLine(players.Count.ToString());
		}

		[Fact]
		public void TestGetRankingInSeason()
		{
			int idSeason = 2021;
			List<Rank> ranks = _externalDataService.GetSeasonRankInSeason();
			_output.WriteLine(ranks.Count.ToString());
		}

		[Fact]
		public void TestGetRoundsInEvent()
		{
			int idEvent = 1014;
			List<EventRound> rounds = _externalDataService.GetRoundsInEvent(idEvent);
			rounds.ForEach(r => _output.WriteLine(r.ToString()));
		}

		[Fact]
		public void TestGetMatchesInEvent()
		{
			int idEvent = 1014;
			List<SnookerBet.Core.Entities.Match> matches = _externalDataService.GetMatchesInEvent(idEvent);
			matches.ForEach(r => _output.WriteLine(r.ToString()));
		}
	}
}
