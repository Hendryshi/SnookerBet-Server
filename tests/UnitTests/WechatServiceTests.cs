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
	public class WechatServiceTests
	{
		private readonly ITestOutputHelper _output;
		private readonly WechatService _wechatService;

		public WechatServiceTests(ITestOutputHelper output)
		{
			_output = output;
			_wechatService = new WechatServiceBuilder().Build();
		}

		[Fact]
		public void TestGetOpenId()	
		{
			string code = "041Z4b0w34JDeY2pi20w3ovQCM2Z4b0-";
			string openId = _wechatService.GetUserOpenId(code);
			_output.WriteLine(openId);
		}

		[Fact]
		public void TestGetAccessToken()
		{
			string token = _wechatService.GetAccessToken();
			_output.WriteLine(token);
		}

		[Fact]
		public void TestSendMessage()
		{
			string token = _wechatService.GetAccessToken();
			_output.WriteLine(token);
		}
	}
}
