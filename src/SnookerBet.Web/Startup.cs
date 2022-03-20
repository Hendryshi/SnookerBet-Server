using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using Hangfire.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MovieManagerWeb;
using Serilog;
using SnookerBet.Core;
using SnookerBet.Core.Enumerations;
using SnookerBet.Core.Interfaces;
using SnookerBet.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnookerBet.Web
{
	public class Startup
	{
		public IConfiguration _config { get; }
		public IWebHostEnvironment _env { get; }

		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			_env = environment;

			// use the default config and add config from appsettings.COMPUTERNAME.json (if it exists)
			var builder = new ConfigurationBuilder()
				.SetBasePath(environment.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
				.AddEnvironmentVariables();

			_config = builder.Build();
		}


		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "SnookerBet.Web", Version = "v1" });
			});

			services.AddHangfire(config =>
				config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseDefaultTypeSerializer()
				.UseMemoryStorage());

			services.AddHangfire(config =>
				config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseDefaultTypeSerializer()
				.UseSqlServerStorage(_config.GetConnectionString("SnookerBetDB"), new SqlServerStorageOptions
				{
					CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
					SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
					QueuePollInterval = TimeSpan.Zero,
					UseRecommendedIsolationLevel = true,
					DisableGlobalLocks = true
				}));

			services.AddHangfireServer();

			services.AddInfrastructureInjection(_config);
			services.AddCoreInjection();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
		{
			if(_env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SnookerBet.Web v1"));

			app.UseRouting();

			app.UseAuthorization();

			app.UseSerilogRequestLogging();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseHangfireDashboard("/jobs", new DashboardOptions
			{
				Authorization = new[]
				{
					new HangfireAuthorizationFilter("admin")
				}
			});

			ConfigureHangfireJob(recurringJobManager, serviceProvider);
		}

		public void ConfigureHangfireJob(IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
		{
			using(var connection = JobStorage.Current.GetConnection())
			{
				foreach(var recurringJob in StorageConnectionExtensions.GetRecurringJobs(connection))
				{
					RecurringJob.RemoveIfExists(recurringJob.Id);
				}
			}

			string recurringJobs = _config.GetSection("HangfireJob").GetValue<string>("RecurringJobs");
			foreach(string recurringJob in recurringJobs.Split(';'))
			{
				HangfireJob hangfireJob;
				string jobName = recurringJob.Split(':')[0].Trim();
				string jobCron = recurringJob.Split(':')[1].Trim();

				if(Enum.TryParse(jobName, true, out hangfireJob))
				{
					switch(hangfireJob)
					{
						case HangfireJob.UpdateEventInfo:
							recurringJobManager.AddOrUpdate(jobName, () => serviceProvider.GetService<IJobService>().UpdateQuizEvent(), jobCron);
							break;
						case HangfireJob.CalculateScore:
							recurringJobManager.AddOrUpdate(jobName, () => serviceProvider.GetService<IJobService>().CalculateGamerScore(), jobCron);
							break;
					}
				}
			}
		}
	}
}
