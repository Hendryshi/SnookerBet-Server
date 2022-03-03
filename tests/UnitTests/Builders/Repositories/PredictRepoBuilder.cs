﻿using SnookerBet.Infrastructure.DbContext;
using SnookerBet.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Builders
{
	public class PredictRepoBuilder
	{
		private DapperContext _dbContext;
		private PlayerRepo _playerRepo;

		public PredictRepoBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
			_playerRepo = new PlayerRepoBuilder().Build();
		}

		public PredictRepo Build()
		{
			return new PredictRepo(_dbContext, _playerRepo);
		}
	}
}
