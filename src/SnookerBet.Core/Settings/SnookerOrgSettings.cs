using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Settings
{
	public class SnookerOrgSettings
	{
		public string BaseAddress { get; set; }
		public int Season { get; set; }
		public string EventUrl { get; set; }
		public string EventsInSeasonUrl { get; set; }
		public string MatchUrl { get; set; }
		public string MatchsInEvtUrl { get; set; }
		public string OngoingMatchUrl { get; set; }
		public string PlayerUrl { get; set; }
		public string PlayersInSeasonUrl { get; set; }
		public string PlayersInEventUrl { get; set; }
		public string RankingUrl { get; set; }
		public string RoundInfoUrl { get; set; }
	}
}
