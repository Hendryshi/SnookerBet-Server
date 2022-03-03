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
using SnookerBet.Core.Helper;

namespace SnookerBet.Core.Services
{
	public class QuizService : IQuizService
	{
		private readonly IEventRepo _eventRepo;
		private readonly ISnookerService _snookerService;
		private readonly IQuizRepo  _quizRepo;
		private readonly IGamerRepo _gamerRepo;
		private readonly IAppLogger<QuizService> _logger;

		public QuizService(IAppLogger<QuizService> logger, IEventRepo eventRepo,
			ISnookerService snookerService,
			IQuizRepo quizRepo,
			IGamerRepo gamerRepo)
		{
			_eventRepo = eventRepo;
			_snookerService = snookerService;
			_quizRepo = quizRepo;
			_gamerRepo = gamerRepo;
			_logger = logger;
		}


		public Quiz CreateQuiz(int idEvent)
		{
			if(_quizRepo.FindByEvent(idEvent) != null)
				throw new ApplicationException($"The quiz for Event[id={idEvent}] has already created");

			Event evt = _snookerService.UpdateEventInfo(idEvent);
			_snookerService.UpdatePlayersInEvent(idEvent);

			int idRoundMin = evt.EventRounds.First().IdRound;

			List<Match> firstRoundMatches = evt.EventMatches.FindAll(m => m.IdRound == idRoundMin);
			if(firstRoundMatches.Find(m => m.Player1Id == Constants.TBD || m.Player2Id == Constants.TBD) != null)
				throw new ApplicationException($"Cannot create the quiz for Event[id={idEvent}]. Still has TBD player in first round");

			return _quizRepo.Save(new Quiz() { IdEvent = idEvent, DtStart = DateTime.Today });
		}

		public oQuizPredict GetQuizPredict(int idEvent, string wechatId)
		{
			Quiz quiz = _quizRepo.FindByEvent(idEvent);
			if(quiz == null)
				throw new ApplicationException($"Cannot find quiz for Event[id={idEvent}] in DB");

			Event evt = _eventRepo.FindById(idEvent, true);
			Gamer gamer = _gamerRepo.FindByEvent(idEvent, wechatId);

			return ConvertHelper.ConvertToQuizPredict(evt, gamer, quiz);
		}
		 
	}
}
