using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnookerBet.Core.Interfaces;
using SnookerBet.Core.Entities;

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
				_logger.LogInformation($"Api GetEventWithMatch called: idEvent[{idEvent}]");
				oEvent outputEvent = _snookerService.GetEventInfoWithMatches(idEvent);
				return Ok(outputEvent);
			}
			catch(Exception ex)
			{
				return HandleError("GetEventWithMatch", ex);
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
