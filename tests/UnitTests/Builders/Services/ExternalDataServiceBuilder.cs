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
	public class ExternalDataServiceBuilder
	{
		private readonly IAppLogger<ExternalDataService> _logger;
		private readonly IOptionsSnapshot<SnookerOrgSettings> _snookerOrgSettings;

		public ExternalDataServiceBuilder()
		{
			_logger = new LoggerBuilder<ExternalDataService>().Build();
			_snookerOrgSettings = new SnookerOrgSettingBuilder().Build();
		}

		public ExternalDataService Build()
		{
			return new ExternalDataService(_logger, _snookerOrgSettings);
		}
	}
}
