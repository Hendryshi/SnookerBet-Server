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
	public class QuizServiceBuilder
	{
		private readonly ISnookerService _snookerService;
		private EventRepo _eventRepo;
		private readonly IQuizRepo _quizRepo;
		private readonly IGamerRepo _gamerRepo;
		private readonly IPredictRepo _predictRepo;
		private readonly IWechatService _wechatService;
		private readonly IOptionsSnapshot<QuizSettings> _quizSettings;
		private readonly IAppLogger<QuizService> _logger;
		
		public QuizServiceBuilder()
		{
			_logger = new LoggerBuilder<QuizService>().Build();
			_snookerService = new SnookerServiceBuilder().Build();
			_eventRepo = new EventRepoBuilder().Build();
			_quizRepo = new QuizRepoBuilder().Build();
			_gamerRepo = new GamerRepoBuilder().Build();
			_predictRepo = new PredictRepoBuilder().Build();
			_quizSettings = new QuizSettingBuilder().Build();
			_wechatService = new WechatServiceBuilder().Build();
		}

		public QuizService Build()
		{
			return new QuizService(_logger, _snookerService, _quizRepo, _gamerRepo, _predictRepo, _wechatService, _quizSettings);
		}
	}
}
