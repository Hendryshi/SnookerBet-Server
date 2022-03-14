using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnookerBet.Core.Settings;
using Moq;

namespace UnitTests.Builders
{
	class QuizSettingBuilder
	{
		private Mock<IOptionsSnapshot<QuizSettings>> _mockQuizSetting;

		public QuizSettingBuilder()
		{
			_mockQuizSetting = new Mock<IOptionsSnapshot<QuizSettings>>();
			QuizSettings setting = new QuizSettings()
			{
				QuarterFinalScore = 30,
				SemiFinalScore = 50,
				FinalScore = 100
			};

			_mockQuizSetting.Setup(ap => ap.Value).Returns(setting);
		}

		public IOptionsSnapshot<QuizSettings> Build()
		{
			return _mockQuizSetting.Object;
		}
	}
}
