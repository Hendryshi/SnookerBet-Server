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
	class WechatSettingBuilder
	{
		private Mock<IOptionsSnapshot<WechatSettings>> _mockWechatSetting;

		public WechatSettingBuilder()
		{
			_mockWechatSetting = new Mock<IOptionsSnapshot<WechatSettings>>();
			WechatSettings setting = new WechatSettings()
			{
				AppId = "wx0f859fd3b55bbae8",
				AppSecret = "c9ce4f8723fa4ce81677aba50c8030cd",
				TemplateId = "EJdD37u3aSIlAsb59OoBx-rrfagW7ThBoVw77P7wKBA",
				State = "developer"
			};

			_mockWechatSetting.Setup(ap => ap.Value).Returns(setting);
		}

		public IOptionsSnapshot<WechatSettings> Build()
		{
			return _mockWechatSetting.Object;
		}
	}
}
