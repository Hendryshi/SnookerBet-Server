using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.JsonObjects
{
	public class oEvent
	{
		public int IdEvent { get; set; }
		public string Name { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public List<oEventRound> oEventRounds { get; set; } = new List<oEventRound>();
	}

	public class oPlayer
	{
		public int IdPlayer { get; set; }
		public string Name { get; set; }
		public int? Rank { get; set; }
		public string Photo { get; set; }
	}

	public class oMatch
	{
		public int IdMatch { get; set; }
		public int IdRound { get; set; }
		public int IdEvent { get; set; }
		public int Number { get; set; }
		public oPlayer Player1 { get; set; }
		public int Score1 { get; set; }
		public oPlayer Player2 { get; set; }
		public int Score2 { get; set; }
		public int WinnerId { get; set; }
		public short IdStatus { get; set; }
		public DateTime? ScheduledDate { get; set; }
		public string note { get; set; }
	}
}
