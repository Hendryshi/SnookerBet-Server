using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnookerBet.Web.Controllers
{
	//TODO: Add serilog api package
	[ApiController]
	[Route("api/quiz")]
	public class QuizController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<SnookerController> _logger;

		public QuizController(ILogger<SnookerController> logger)
		{
			_logger = logger;
		}

		
	}
}
