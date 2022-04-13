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
using System.Transactions;
using SnookerBet.Core.Settings;
using Microsoft.Extensions.Options;
using SnookerBet.Core.Extensions;

namespace SnookerBet.Core.Services
{
	public class JobService : IJobService
	{
		private readonly ISnookerService _snookerService;
		private readonly IQuizService  _quizService;
		private readonly IAppLogger<JobService> _logger;

		public JobService(IAppLogger<JobService> logger,
			ISnookerService snookerService,
			IQuizService quizService)
		{
			_snookerService = snookerService;
			_quizService = quizService;
			_logger = logger;
		}

		public void UpdateQuizEvent()
		{
			Quiz quiz = _quizService.GetCurrentQuiz();

			if(quiz != null && quiz.IdStatus != QuizStatus.Done && quiz.IdStatus != QuizStatus.OpenPredict)
				_snookerService.UpdateEventInfo(quiz.IdEvent);
		}

		public void CalculateGamerScore()
		{
			Quiz quiz = _quizService.GetCurrentQuiz();
			if(quiz.IdStatus != QuizStatus.Done)
			{
				_quizService.CalculateGamerScore();
				Event curEvent = _snookerService.GetEventById(quiz.IdEvent, true);
				if(!curEvent.EventMatches.Exists(m => m.EndDate == null))
				{
					_logger.LogInformation($"All matches have been finished in event {quiz.IdEvent}. Update Quiz Status to DONE");
					quiz.DtEnd = DateTime.Now;
					quiz.IdStatus = QuizStatus.Done;
					_quizService.Save(quiz);
				}
			}
		}
	}
}
