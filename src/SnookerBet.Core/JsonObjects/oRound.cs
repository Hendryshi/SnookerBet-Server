using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.JsonObjects
{

	public class oRound
	{
		public int IdRound { get; set; }
		public string RoundName { get; set; }
		public int Distance { get; set; }
		public int Money { get; set; }
		public string Currency { get; set; }
	}

	public class oQuizRound : oRound
	{
		public List<oPredict> oPredicts { get; set; } = new List<oPredict>();
	}

	public class oEventRound : oRound
	{
		public List<oMatch> oMatches { get; set; } = new List<oMatch>();
	}
}
