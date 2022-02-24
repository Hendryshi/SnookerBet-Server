using System;
using Xunit;
using SnookerBet.Infrastructure.Services;


namespace UnitTests
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			HtmlService _htmlService = new HtmlService(null);
			_htmlService.GetMatchsInEvent(1140);
		}
	}
}
