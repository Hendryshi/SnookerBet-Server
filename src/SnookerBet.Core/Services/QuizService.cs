using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SnookerBet.Core.Interfaces;
using SnookerBet.Core.Entities;
using SnookerBet.Core.Enumerations;
using SnookerBet.Core.JsonObjects;

namespace SnookerBet.Core.Services
{
	public class QuizService : IQuizService
	{
		private readonly IEventRepo _eventRepo;
		private readonly ISnookerService _snookerService;
		private readonly IQuizRepo  _quizRepo;
		private readonly IExternalDataService _externalDataService;
		private readonly IAppLogger<SnookerService> _logger;

		public QuizService(IAppLogger<SnookerService> logger, IEventRepo eventRepo,
			ISnookerService snookerService,
			IQuizRepo quizRepo,
			IExternalDataService externalDataService)
		{
			_eventRepo = eventRepo;
			_snookerService = snookerService;
			_quizRepo = quizRepo;
			_externalDataService = externalDataService;
			_logger = logger;
		}


		public void UpdateEventsInSeason(int season)
		{
			_logger?.LogInformation("Start update all events info in season {0} ", season);
			List<Event> events = _externalDataService.GetEventsInSeason(season)?.FindAll(e => e.TyEvent == "Ranking");

			if(events != null && events.Count > 0)
			{
				_eventRepo.SaveList(events);
				_logger?.LogInformation("{0} events in season {1} have been updated", events.Count, season);
			}
			else
			{
				_logger?.LogWarning("Cannot find any ranking events in season {0}", season);
			}
		}

		public Quiz CreateQuiz(int idEvent)
		{
			if(_quizRepo.FindByEvent(idEvent) != null)
				throw new ApplicationException($"The quiz for Event[id={idEvent}] has already created");

			Event evt = _snookerService.UpdateEventInfo(idEvent);

			int idRoundMin = evt.EventRounds.First().IdRound;

			List<Match> firstRoundMatches = evt.EventMatches.FindAll(m => m.IdRound == idRoundMin);
			if(firstRoundMatches.Find(m => m.Player1Id == Constants.TBD || m.Player2Id == Constants.TBD) != null)
				throw new ApplicationException($"Cannot create the quiz for Event[id={idEvent}]. Still has TBD player in first round");

			return _quizRepo.Save(new Quiz() { IdEvent = idEvent, DtStart = DateTime.Today });
		}

		public oQuizPredict GetQuizPredict(int idEvent, string wechatId)
		{
			Quiz quiz = _quizRepo.FindByEvent(idEvent);
			Event evt = _snookerService.UpdateEventInfo(idEvent);
			

			return null;
		}
		 
	}
}
