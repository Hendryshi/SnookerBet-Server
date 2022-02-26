using Microsoft.Extensions.Logging;
using SnookerBet.Infrastructure.Logging;
using SnookerBet.Infrastructure.Repositories;
using SnookerBet.Infrastructure.DbContext;
using SnookerBet.Core.Services;
using SnookerBet.Core.Interfaces;
using Microsoft.Extensions.Options;
using SnookerBet.Core.Settings;

namespace UnitTests.Builders
{
	public class SnookerServiceBuilder
	{
		private EventRepo _eventRepo;
		private PlayerRepo _playerRepo;
		private EventRoundRepo _eventRoundRepo;
		private MatchRepo _matchRepo;
		private ExternalDataService _externalDataService;
		private DapperContext _dbContext;
		private readonly IAppLogger<SnookerService> _logger;
		
		public SnookerServiceBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
			_logger = new LoggerBuilder<SnookerService>().Build();
			_externalDataService = new ExternalDataServiceBuilder().Build();
			_eventRoundRepo = new EventRoundRepo(_dbContext);
			_matchRepo = new MatchRepo(_dbContext);
			_eventRepo = new EventRepo(_dbContext, _eventRoundRepo, _matchRepo);
			_playerRepo = new PlayerRepo(_dbContext);
		}

		public SnookerService Build()
		{
			return new SnookerService(_logger, _eventRepo, _playerRepo, _externalDataService);
		}
	}
}
