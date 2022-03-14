using SnookerBet.Core.Entities;
using SnookerBet.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnookerBet.Core.JsonObjects
{
	public class oEvent : BaseEntity
	{
		public int IdEvent { get; set; }
		public string Name { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public List<oEventRound> oEventRounds { get; set; } = new List<oEventRound>();

		[JsonConstructor]
		public oEvent() { }

	}

	public class oPlayer : BaseEntity
	{
		public int IdPlayer { get; set; }
		public string Name { get; set; }
		public int? Rank { get; set; }
		public string Photo { get; set; }

		[JsonConstructor]
		public oPlayer() { }
	}

	public class oMatch : BaseEntity
	{
		public int IdEvent { get; set; }
		public int IdRound { get; set; }
		public string RoundName { get; set; }
		public int Number { get; set; }
		public oPlayer Player1 { get; set; }
		public int Score1 { get; set; }
		public oPlayer Player2 { get; set; }
		public int Score2 { get; set; }
		public int WinnerId { get; set; }
		public MatchStatus StMatch { get; set; }
		public DateTime? ScheduledDate { get; set; }
		public string note { get; set; }

		public oMatch(oMatch m)
		{
			this.IdEvent = m.IdEvent;
			this.IdRound = m.IdRound;
			this.RoundName = m.RoundName;
			this.Number = m.Number;
			Player1 = m.Player1;
			Score1 = m.Score1;
			Player2 = m.Player2;
			Score2 = m.Score2;
			WinnerId = m.WinnerId;
			StMatch = m.StMatch;
			ScheduledDate = m.ScheduledDate;
			note = m.note;
		}

		[JsonConstructor]
		public oMatch() { }
	}
}
