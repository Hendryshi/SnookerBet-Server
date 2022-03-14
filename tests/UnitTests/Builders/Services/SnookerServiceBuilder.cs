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
		private EventRoundRepo _roundRepo;
		private MatchRepo _matchRepo;
		private PlayerRepo _playerRepo;
		private ExternalDataService _externalDataService;
		private readonly IAppLogger<SnookerService> _logger;
		
		public SnookerServiceBuilder()
		{
			_logger = new LoggerBuilder<SnookerService>().Build();
			_externalDataService = new ExternalDataServiceBuilder().Build();
			_eventRepo = new EventRepoBuilder().Build();
			_roundRepo = new RoundRepoBuilder().Build();
			_matchRepo = new MatchRepoBuilder().Build();
			_playerRepo = new PlayerRepoBuilder().Build();
		}

		public SnookerService Build()
		{
			return new SnookerService(_logger, _eventRepo, _roundRepo, _matchRepo, _playerRepo, _externalDataService);
		}
	}
}
