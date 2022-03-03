using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SnookerBet.Core.Interfaces;
using SnookerBet.Core.Settings;


namespace SnookerBet.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureInjection(this IServiceCollection services, IConfiguration _config)
		{
			services.Configure<SnookerOrgSettings>(_config.GetSection("SnookerOrgSettings"));

			services.AddScoped<DbContext.DapperContext>();
			services.AddScoped(typeof(IAppLogger<>), typeof(Logging.LoggerAdapter<>));
			services.AddTransient<IEventRepo, Repositories.EventRepo>();
			services.AddTransient<IEventRoundRepo, Repositories.EventRoundRepo>();
			services.AddTransient<IMatchRepo, Repositories.MatchRepo>();
			services.AddTransient<IPlayerRepo, Repositories.PlayerRepo>();
			services.AddTransient<IQuizRepo, Repositories.QuizRepo>();
			services.AddTransient<IGamerRepo, Repositories.GamerRepo>();
			services.AddTransient<IPredictRepo, Repositories.PredictRepo>();
			return services;
		}
	}
}
