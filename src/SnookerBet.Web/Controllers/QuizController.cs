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
				string wechatId = Request.Headers.FirstOrDefault(x => x.Key == "WechatId").Value.FirstOrDefault();
				_logger.LogInformation($"Api GetQuizEvent called - [{wechatId}]");
				return Ok(_quizService.GetAvailableQuiz());
			}
			catch(Exception ex)
			{
				return HandleError("GetQuizEvent", ex);
			}
		}


		[HttpGet]
		[Route("GetQuizPredict")]
		public IActionResult GetQuizPredict(int e, string wn, bool re = false)
		{
			try
			{
				string wechatId = Request.Headers.FirstOrDefault(x => x.Key == "WechatId").Value.FirstOrDefault();
				_logger.LogInformation($"Api GetQuizPredict called: idEvent[{e}] - wechatName[{wn}] - isReEdit[{re}] - [{wechatId}]");
				oQuizPredict quizPredict = _quizService.GetQuizPredict(e, wn, re);
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
				string wechatId = Request.Headers.FirstOrDefault(x => x.Key == "WechatId").Value.FirstOrDefault();
				_logger.LogInformation($"Api GetQuizSummary called - [{wechatId}]");
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
				string wechatId = Request.Headers.FirstOrDefault(x => x.Key == "WechatId").Value.FirstOrDefault();
				_logger.LogInformation($"Api GetQuizTrending called - [{wechatId}]");
				List<oPredictGamerTrend> oPredictGamerTrends = _quizService.GetPredictTrending(idEvent);
				return Ok(oPredictGamerTrends);
			}
			catch(Exception ex)
			{
				return HandleError("GetQuizTrending", ex);
			}
		}

		[HttpGet]
		[Route("GetQuizMatch")]
		public IActionResult GetQuizMatch(int e, int r, int n)
		{
			try
			{
				string wechatId = Request.Headers.FirstOrDefault(x => x.Key == "WechatId").Value.FirstOrDefault();
				_logger.LogInformation($"Api GetQuizMatch called - [{wechatId}]");
				oQuizMatch quizMatch = _quizService.GetQuizMatch(e,r,n);
				return Ok(quizMatch);
			}
			catch(Exception ex)
			{
				return HandleError("GetQuizMatch", ex);
			}
		}

		[HttpGet]
		[Route("GetQuizReport")]
		public IActionResult GetQuizReport(int? idEvent = null)
		{
			try
			{
				string wechatId = Request.Headers.FirstOrDefault(x => x.Key == "WechatId").Value.FirstOrDefault();
				_logger.LogInformation($"Api GetQuizReport called idEvent[{idEvent}] - [{wechatId}]");
				return Ok(_quizService.GetQuizSummary(idEvent));
			}
			catch(Exception ex)
			{
				return HandleError("GetQuizReport", ex);
			}
		}

		[HttpPost]
		[Route("CreateQuiz/{idEvent}")]
		public IActionResult CreateQuiz(int idEvent)
		{
			try
			{
				string wechatId = Request.Headers.FirstOrDefault(x => x.Key == "WechatId").Value.FirstOrDefault();
				_logger.LogInformation($"Api CreateQuiz called: idEvent[{idEvent}] - [{wechatId}]");
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
				string wechatId = Request.Headers.FirstOrDefault(x => x.Key == "WechatId").Value.FirstOrDefault();
				_logger.LogInformation($"Api UpdateQuizPredict called - [{wechatId}]");

				if(quizPredict == null || quizPredict.oGamer == null || string.IsNullOrEmpty(quizPredict.oGamer.GamerName))
					throw new ApplicationException("quizPredict not validate");

				_quizService.UpdateQuizPredict(quizPredict);
				return Ok("QuizPredict has been saved in DB");
			}
			catch(Exception ex)
			{
				return HandleError("UpdateQuizPredict", ex);
			}
		}

		[HttpPost]
		[Route("CalculateGamerScore")]
		public IActionResult CalculateGamerScore(int idEvent = 0, DateTime? dtStamp = null)
		{
			try
			{
				string wechatId = Request.Headers.FirstOrDefault(x => x.Key == "WechatId").Value.FirstOrDefault();
				_logger.LogInformation($"Api CalculateGamerScore called - idEvent={idEvent} - dtStamp={dtStamp} - [{wechatId}]");
				_quizService.CalculateGamerScore(idEvent, dtStamp);
				return Ok("Calculate gamer score finished");
			}
			catch(Exception ex)
			{
				return HandleError("CalculateGamerScore", ex);
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		public IActionResult HandleError(string apiName, Exception ex)
		{
			_logger.LogError(ex, $"Error occurred in Api {apiName}");

			if(ex is ApplicationException)
				return Problem(statusCode: 503, detail: ex.Message);
			else
				return Problem(statusCode: 500, detail: ex.Message);
		}
	}
}
