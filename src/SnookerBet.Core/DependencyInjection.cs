using Microsoft.Extensions.DependencyInjection;
using SnookerBet.Core.Interfaces;

namespace SnookerBet.Core
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCoreInjection(this IServiceCollection services)
		{

			services.AddTransient<IExternalDataService, Services.ExternalDataService>();
			services.AddTransient<ISnookerService, Services.SnookerService>();
			services.AddTransient<IQuizService, Services.QuizService>();
			return services;
		}
	}
}
