using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Enumerations
{
	public enum HangfireJob : short
	{
		UpdateEventInfo = 1,
		CalculateScore
	}
}
