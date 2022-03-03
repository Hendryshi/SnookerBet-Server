using SnookerBet.Infrastructure.DbContext;
using SnookerBet.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Builders
{
	public class PlayerRepoBuilder
	{
		private DapperContext _dbContext;

		public PlayerRepoBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
		}

		public PlayerRepo Build()
		{
			return new PlayerRepo(_dbContext);
		}
	}
}
