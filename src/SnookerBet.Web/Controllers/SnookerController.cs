using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnookerBet.Core.Interfaces;
using SnookerBet.Core.Entities;
using System.Threading;
using SnookerBet.Core.JsonObjects;

namespace SnookerBet.Web.Controllers
{
	[ApiController]
	[Route("api/[Controller]")]
	public class SnookerController : ControllerBase
	{
		
		private readonly ILogger<SnookerController> _logger;
		private readonly ISnookerService _snookerService;

		public SnookerController(ILogger<SnookerController> logger, ISnookerService snookerService)
		{
			_logger = logger;
			_snookerService = snookerService;
		}



		[HttpGet]
		[Route("GetEventWithMatch/{idEvent}")]
		public IActionResult GetEventWithMatch(int idEvent)
		{
			try
			{
				string wechatId = Request.Headers.FirstOrDefault(x => x.Key == "WechatId").Value.FirstOrDefault();
				_logger.LogInformation($"Api GetEventWithMatch called: idEvent[{idEvent}] - [{wechatId}]");
				oEvent outputEvent = _snookerService.GetEventInfoWithMatches(idEvent);
				return Ok(outputEvent);
			}
			catch(Exception ex)
			{
				return HandleError("GetEventWithMatch", ex);
			}
		}
		
		[HttpGet]
		[Route("GetOnGoingMatch")]
		public IActionResult GetOnGoingMatch()
		{
			try
			{
				string wechatId = Request.Headers.FirstOrDefault(x => x.Key == "WechatId").Value.FirstOrDefault();
				_logger.LogInformation($"Api GetOnGoingMatch called - [{wechatId}]");
				List<oMatch> oMatches = _snookerService.GetOnGoingMatch();
				return Ok(oMatches);
			}
			catch(Exception ex)
			{
				return HandleError("GetOnGoingMatch", ex);
			}
		}

		[HttpPost]
		[Route("UpdatePlayer/{idPlayer}")]
		public IActionResult UpdatePlayerById(int idPlayer)
		{
			try
			{
				_logger.LogInformation($"Api UpdatePlayerById called: idPlayer[{idPlayer}]");
				_snookerService.UpdatePlayerById(idPlayer);
				return Ok($"Player[{idPlayer}] has been updated in DB");
			}
			catch(Exception ex)
			{
				return HandleError("GetEventWithMatch", ex);
			}
		}

		[HttpPost]
		[Route("UpdateEventsInSeason/{season}")]
		public IActionResult UpdateEventsInSeason(int season)
		{
			try
			{
				_logger.LogInformation($"Api UpdateEventsInSeason called: season[{season}]");
				_snookerService.UpdateEventsInSeason(season);
				return Ok($"All Events in season {season} have been updated in DB");
			}
			catch(Exception ex)
			{
				return HandleError("UpdateEventsInSeason", ex);
			}
		}

		[HttpPost]
		[Route("UpdatePlayersInSeason/{season}")]
		public IActionResult UpdatePlayersInSeason(int season)
		{
			try
			{
				_logger.LogInformation($"Api UpdatePlayersInSeason called: season[{season}]");
				_snookerService.UpdatePlayersInSeason(season);
				return Ok($"All players in season {season} have been updated in DB");
			}
			catch(Exception ex)
			{
				return HandleError("UpdatePlayersInSeason", ex);
			}
		}

		[HttpPost]
		[Route("UpdateEvent/{idEvent}")]
		public IActionResult UpdateEventInfo(int idEvent)
		{
			try
			{
				_logger.LogInformation($"Api UpdateEventInfo called: season[{idEvent}]");	
				_snookerService.UpdateEventInfo(idEvent);
				return Ok($"Event[{idEvent}] and its matches have been updated in DB");
			}
			catch(Exception ex)
			{
				return HandleError("UpdateEventInfo", ex);
			}
		}

		//TODO: Get Today Match

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
