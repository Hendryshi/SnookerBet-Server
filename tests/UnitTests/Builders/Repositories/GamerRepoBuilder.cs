using SnookerBet.Infrastructure.DbContext;
using SnookerBet.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Builders
{
	public class GamerRepoBuilder
	{
		private DapperContext _dbContext;
		private PredictRepo _predictRepo;

		public GamerRepoBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
			_predictRepo = new PredictRepoBuilder().Build();
		}

		public GamerRepo Build()
		{
			return new GamerRepo(_dbContext, _predictRepo);
		}
	}
}
