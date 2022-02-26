using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnookerBet.Core.Settings;
using Moq;
using Microsoft.Extensions.Logging;
using SnookerBet.Infrastructure.Logging;
using Serilog;

namespace UnitTests.Builders
{
	public class LoggerBuilder<T>
	{
		private ILoggerFactory _loggerFactory;

		public LoggerBuilder(string logPath = "")
		{
			if(string.IsNullOrEmpty(logPath))
				logPath = @"D:\WorkSpace\Logs\SnookerBet\Test\log.txt";

			_loggerFactory = LoggerFactory.Create(builder => builder.AddFile(logPath));
		}

		public LoggerAdapter<T> Build()
		{
			return new LoggerAdapter<T>(_loggerFactory);
		}
	}
}
