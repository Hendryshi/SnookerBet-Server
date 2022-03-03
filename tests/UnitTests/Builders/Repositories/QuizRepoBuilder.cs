using SnookerBet.Infrastructure.DbContext;
using SnookerBet.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Builders
{
	public class QuizRepoBuilder
	{
		private DapperContext _dbContext;

		public QuizRepoBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
		}

		public QuizRepo Build()
		{
			return new QuizRepo(_dbContext);
		}
	}
}
