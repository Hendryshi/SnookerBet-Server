using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SnookerBet.Core.Interfaces;
using SnookerBet.Core.JsonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

		
		[HttpGet]
		[Route("GetQuizEvent")]
		public IActionResult GetQuizEvent()
		{
			try
			{
				_logger.LogInformation($"Api GetQuizEvent called");
				List<oQuiz> quiz = _quizService.GetAvailableQuiz();
				return Ok(quiz);
			}
			catch(Exception ex)
			{
				return HandleError("GetQuizEvent", ex);
			}
		}


		[HttpGet]
		[Route("GetQuizPredict")]
		public IActionResult GetQuizPredict(int e, string wn)
		{
			try
			{
				_logger.LogInformation($"Api GetQuizPredict called: idEvent[{e}] - wechatName[{wn}]");
				oQuizPredict quizPredict = _quizService.GetQuizPredict(e, wn);
				return Ok(quizPredict);
			}
			catch(Exception ex)
			{
				return HandleError("GetQuizPredict", ex);
			}
		}

		[HttpGet]
		[Route("GetQuizSummary")]
		public IActionResult GetQuizSummary(int? idEvent = null)
		{
			try
			{
				_logger.LogInformation($"Api GetQuizSummary called");
				List<oPredictStat> oPredictStats = _quizService.GetPredictSummary(idEvent);
				return Ok(oPredictStats);
			}
			catch(Exception ex)
			{
				return HandleError("GetQuizSummary", ex);
			}
		}

		[HttpGet]
		[Route("GetQuizTrending")]
		public IActionResult GetQuizTrending(int? idEvent = null)
		{
			try
			{
				_logger.LogInformation($"Api GetQuizTrending called");
				List<oPredictGamerTrend> oPredictGamerTrends = _quizService.GetPredictTrending(idEvent);
				return Ok(oPredictGamerTrends);
			}
			catch(Exception ex)
			{
				return HandleError("GetQuizTrending", ex);
			}
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

		[HttpPost]
		[Route("UpdateQuizPredict")]
		public IActionResult UpdateQuizPredict(oQuizPredict quizPredict)
		{
			try
			{
				_logger.LogInformation($"Api UpdateQuizPredict called");
				_quizService.UpdateQuizPredict(quizPredict);
				return Ok("QuizPredict has been saved in DB");
			}
			catch(Exception ex)
			{
				return HandleError("UpdateQuizPredict", ex);
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
