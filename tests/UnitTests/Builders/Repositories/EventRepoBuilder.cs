using SnookerBet.Core.Entities;
using SnookerBet.Infrastructure.DbContext;
using SnookerBet.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Builders
{
	public class EventRepoBuilder
	{
		private DapperContext _dbContext;
		private EventRoundRepo _eventRoundRepo;
		private MatchRepo _matchRepo;

		public EventRepoBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
			_eventRoundRepo = new RoundRepoBuilder().Build();
			_matchRepo = new MatchRepoBuilder().Build();
		}

		public EventRepo Build()
		{
			return new EventRepo(_dbContext, _eventRoundRepo, _matchRepo);
		}
	}
}
