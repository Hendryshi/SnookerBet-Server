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
using SnookerBet.Infrastructure.Repositories;
using SnookerBet.Infrastructure.DbContext;
using SnookerBet.Core.JsonObjects;

namespace UnitTests
{
	public class SnookerServiceTests
	{
		private readonly ITestOutputHelper _output;
		private readonly SnookerService _snookerService;

		public SnookerServiceTests(ITestOutputHelper output)
		{
			_output = output;
			_snookerService = new SnookerServiceBuilder().Build();
		}

		[Fact]
		public void TestUpdateEventsInSeason()	
		{
			int idSeason = 0;
			_snookerService.UpdateEventsInSeason(idSeason);
		}

		[Fact]
		public void TestUpdatePlayersInSeason()
		{
			int idSeason = 0;
			_snookerService.UpdatePlayersInSeason(idSeason);
		}

		[Fact]
		public void TestUpdateRounds()
		{
			EventRound round = new EventRound() { IdEvent = 1128, IdRound = 5, RoundName = "Round 9", Distance = 10, NumLeft = 144, NumMatches = 32, Rank = 160 };
			EventRoundRepo roundRepo = new EventRoundRepo(new DapperContext(new ConfigBuilder().Build()));
			roundRepo.Save(round);
		}

		[Fact]
		public void TestUpdateEventInfo()
		{
			int idEvent = 1154;
			_snookerService.UpdateEventInfo(idEvent);
		}

		[Fact]
		public void TestGetOEventInfoWithMatches()
		{                                 
			int idEvent = 1134;
			oEvent evt = _snookerService.GetEventInfoWithMatches(idEvent);
			_output.WriteLine(evt.ToString());
		}

		[Fact]
		public void TestGetEventWithMatch()
		{
			EventRepo eventRepo = new EventRepoBuilder().Build();
			int idEvent = 1134;
			
			eventRepo.FindById(idEvent, false);
		}

		[Fact]
		public void TestGetEndedMatchInDay()
		{
			MatchRepo matchRepo = new MatchRepoBuilder().Build();
			DateTime date = new DateTime(2022, 3, 2);

			List<SnookerBet.Core.Entities.Match> matches = matchRepo.GetEndedMatchInDay(0, date);
			Assert.Equal(19, matches.Count);
		}
	}
}
