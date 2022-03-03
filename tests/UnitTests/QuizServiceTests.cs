using System;
using Xunit;
using Moq;
using UnitTests.Builders;
using Microsoft.Extensions.Options;
using SnookerBet.Core.Services;
using SnookerBet.Core.Settings;
using SnookerBet.Core.Entities;
using SnookerBet.Infrastructure.Logging;
using System.Collections.Generic;
using Xunit.Abstractions;
using System.Linq;
using SnookerBet.Infrastructure.Repositories;
using SnookerBet.Infrastructure.DbContext;
using SnookerBet.Core.JsonObjects;

namespace UnitTests
{
	public class QuizServiceTests
	{
		private readonly ITestOutputHelper _output;
		private readonly QuizService _quizService;

		public QuizServiceTests(ITestOutputHelper output)
		{
			_output = output;
			_quizService = new QuizServiceBuilder().Build();
		}

		[Fact]
		public void TestFindGamerByEvent()	
		{
			GamerRepo gamerRepo = new GamerRepoBuilder().Build();

			int idEvent = 1134;
			string name = "yejia";

			Gamer gamer = gamerRepo.FindByEvent(idEvent, name);
			Assert.Null(gamer);
		}

		 [Fact]
		public void TestGetQuizPredict()
		{
			int idEvent = 1134;
			string wechatId = "Yejia";
			
			oQuizPredict quizPredict = _quizService.GetQuizPredict(idEvent, wechatId);
		}

	}
}
