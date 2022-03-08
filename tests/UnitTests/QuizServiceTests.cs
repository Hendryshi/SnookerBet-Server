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
using SnookerBet.Core.Enumerations;

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

			Gamer gamer = gamerRepo.FindByEventAndName(idEvent, name);
			Assert.Null(gamer);
		}

		 [Fact]
		public void TestGetQuizPredict()
		{
			int idEvent = 1134;
			string wechatId = "Yejia";
			
			oQuizPredict quizPredict = _quizService.GetQuizPredict(idEvent, wechatId);
		}

		[Fact]
		public void TestUpdateQuizPredict()
		{
			int idEvent = 1134;
			string wechatId = "FeiFei";

			oQuizPredict quizPredict = _quizService.GetQuizPredict(idEvent, wechatId);
			quizPredict.oGamer.gamerName = "FeiFei";
			quizPredict.oGamer.wechatName = wechatId;

			_quizService.UpdateQuizPredict(quizPredict);
		}

		[Fact]
		public void TestLoadPredictList()
		{
			int idEvent = 0;
			int idGamer = 0;
			PredictRepo predictRepo = new PredictRepoBuilder().Build();

			List<Predict> predicts = predictRepo.LoadPredictsByEventAndGamer(idEvent, idGamer);
			Assert.Empty(predicts);
		}

		[Fact]
		public void TestGetQuizFindByStatus()
		{
			List<QuizStatus> quizStatuses = new List<QuizStatus>() { QuizStatus.OpenPredict, QuizStatus.ReOpenPredict };
			QuizRepo quizRepo = new QuizRepoBuilder().Build();

			List<Quiz> quizzes = quizRepo.FindByStatus(quizStatuses);
			Assert.Single(quizzes);
		}

		[Fact]
		public void TestGetAvailableQuiz()
		{
			List<oQuiz> oQuizzes = _quizService.GetAvailableQuiz();
			Assert.Single(oQuizzes);
			oQuizzes.ForEach(q => _output.WriteLine(q.ToString()));
		}

		[Fact]
		public void TestGetOQuizMatch()
		{
			int idEvent = 1134;
			int idRound = 8;
			int number = 2;

			oQuizMatch quizMatch = _quizService.GetQuizMatch(idEvent, idRound, number);
			_output.WriteLine(quizMatch.ToString());
		}

		[Fact]
		public void TestGetQuizSummary()
		{
			List<oPredictStat> oPredictSummarys = _quizService.GetPredictSummary();
			oPredictSummarys.ForEach(p => _output.WriteLine(p.ToString()));
		}

		[Fact]
		public void TestGetPredictPointByDay()
		{
			int idEvent = 1134;
			int idGamer = 2;
			QuizRepo quizRepo = new QuizRepoBuilder().Build();
			List<oPredictByDay> oPredictByDays = quizRepo.GetPredictPointByDay(idEvent, idGamer);
			oPredictByDays.ForEach(p => _output.WriteLine(p.ToString()));
		}

		[Fact]
		public void TestGetPredictTrending()
		{
			int idEvent = 1014;
			List<oPredictGamerTrend> oPredictGamerTrends = _quizService.GetPredictTrending(idEvent);
			oPredictGamerTrends.ForEach(p => _output.WriteLine(p.ToString()));
		}

		[Fact]
		public void TestMath()
		{
			decimal a = Math.Round((decimal)100/6, 2);
			decimal b = a * 3;
			_output.WriteLine(b.ToString());
		}

		[Fact]
		public void TestCalculatePoint()
		{
			_quizService.CalculateGamerScore();
		}
	}
}
