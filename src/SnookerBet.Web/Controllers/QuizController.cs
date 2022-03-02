using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SnookerBet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnookerBet.Web.Controllers
{
	//TODO: Add serilog api package
	[ApiController]
	[Route("api/[Controller]")]
	public class QuizController : ControllerBase
	{
		private readonly ILogger<QuizController> _logger;
		private readonly IQuizService _quizService;

		public QuizController(ILogger<QuizController> logger, IQuizService quizService)
		{
			_logger = logger;
			_quizService = quizService;
		}

		[HttpPost]
		[Route("CreateQuiz/{idEvent}")]
		public IActionResult CreateQuiz(int idEvent)
		{
			try
			{
				_logger.LogInformation($"Api CreateQuiz called: idEvent[{idEvent}]");
				_quizService.CreateQuiz(idEvent);
				return Ok($"Quiz for event[{idEvent}] has been created in DB");
			}
			catch(Exception ex)
			{
				return HandleError("CreateQuiz", ex);
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		public IActionResult HandleError(string apiName, Exception ex)
		{
			_logger.LogError(ex, $"Error occurred in Api {apiName}");

			return Problem(statusCode: 500, detail: ex.Message);
		}
	}
}
