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
	class SnookerOrgSettingBuilder
	{
		private Mock<IOptionsSnapshot<SnookerOrgSettings>> _mockSnookerOrgSetting;

		public SnookerOrgSettingBuilder()
		{
			_mockSnookerOrgSetting = new Mock<IOptionsSnapshot<SnookerOrgSettings>>();
			SnookerOrgSettings setting = new SnookerOrgSettings()
			{
				BaseAddress = "http://api.snooker.org/",
				Season = 2021,
				EventUrl = "http://api.snooker.org/?e={0}",
				EventsInSeasonUrl = "http://api.snooker.org/?t=5&s={0}",
				MatchUrl = "http://api.snooker.org/?e={0}&r={1}&n={2}",
				MatchsInEvtUrl = "http://api.snooker.org/?t=6&e={0}",
				OngoingMatchUrl = "http://api.snooker.org/?t=7",
				PlayerUrl = "http://api.snooker.org/?p={0}",
				PlayersInsSeasonUrl = "http://api.snooker.org/?t=10&st=p&s={0}",
				RankingUrl = "http://api.snooker.org/?rt=MoneyRankings&s={0}",
				RoundInfoUrl = "http://api.snooker.org/?t=12&e={0}"
			};

			_mockSnookerOrgSetting.Setup(ap => ap.Value).Returns(setting);
		}

		public IOptionsSnapshot<SnookerOrgSettings> Build()
		{
			return _mockSnookerOrgSetting.Object;
		}
	}
}
