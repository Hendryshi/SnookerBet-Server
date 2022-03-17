using SnookerBet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnookerBet.Core.JsonObjects
{

	public class oRound : BaseEntity
	{
		public int IdRound { get; set; }
		public string RoundName { get; set; }
		public int Distance { get; set; }
		public int Money { get; set; }
		public string Currency { get; set; }

		public oRound(oRound r)
		{
			this.IdRound = r.IdRound;
			this.RoundName = r.RoundName;
			this.Distance = r.Distance;
			this.Money = r.Money;
			this.Currency = r.Currency;
		}

		public oRound() { }
	}

	public class oQuizRound : oRound
	{
		public List<oPredict> oPredicts { get; set; } = new List<oPredict>();

		public oQuizRound(oRound r) : base(r) { }
		
		[JsonConstructor]
		public oQuizRound() { }
	}

	public class oEventRound : oRound
	{
		public List<oMatch> oMatches { get; set; } = new List<oMatch>();
		public bool NoTBDMatch { get; set; }

		public oEventRound(oRound r) : base(r) { }

		[JsonConstructor]
		public oEventRound() { }
	}
}
