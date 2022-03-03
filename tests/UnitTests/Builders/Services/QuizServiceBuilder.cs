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
		private readonly IAppLogger<QuizService> _logger;
		
		public QuizServiceBuilder()
		{
			_logger = new LoggerBuilder<QuizService>().Build();
			_snookerService = new SnookerServiceBuilder().Build();
			_eventRepo = new EventRepoBuilder().Build();
			_quizRepo = new QuizRepoBuilder().Build();
			_gamerRepo = new GamerRepoBuilder().Build();
		}

		public QuizService Build()
		{
			return new QuizService(_logger, _eventRepo, _snookerService, _quizRepo, _gamerRepo);
		}
	}
}
