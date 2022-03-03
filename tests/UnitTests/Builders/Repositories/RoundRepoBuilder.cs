using SnookerBet.Infrastructure.DbContext;
using SnookerBet.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Builders
{
	public class RoundRepoBuilder
	{
		private DapperContext _dbContext;

		public RoundRepoBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
		}

		public EventRoundRepo Build()
		{
			return new EventRoundRepo(_dbContext);
		}
	}
}
