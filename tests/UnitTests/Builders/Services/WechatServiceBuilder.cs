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
	public class WechatServiceBuilder
	{
		private readonly IAppLogger<WechatService> _logger;
		private readonly IOptionsSnapshot<WechatSettings> _wechatSettings;

		public WechatServiceBuilder()
		{
			_logger = new LoggerBuilder<WechatService>().Build();
			_wechatSettings = new WechatSettingBuilder().Build();
		}

		public WechatService Build()
		{
			return new WechatService(_logger, _wechatSettings);
		}
	}
}
