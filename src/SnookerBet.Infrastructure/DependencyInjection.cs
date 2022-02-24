using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace SnookerBet.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureInjection(this IServiceCollection services, IConfiguration _config)
		{
			//services.AddTransient<IMovieHistoryRepo, Repositories.MovieHistoryRepo>();
			return services;
		}
	}
}
