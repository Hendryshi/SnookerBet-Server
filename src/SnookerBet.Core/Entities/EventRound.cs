using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Entities
{
	[Table("S_EventRound")]
	public class EventRound : BaseEntity
	{
		[ExplicitKey]
		[JsonProperty("EventID")]
		public int IdEvent { get; set; }
		[ExplicitKey]
		[JsonProperty("Round")]
		public int IdRound { get; set; }
		public string RoundName { get; set; }
		public int Distance { get; set; }
		public int NumLeft { get; set; }
		public int NumMatches { get; set; }
		public int Rank { get; set; }
		public int Money { get; set; }
		public int ActualMoney { get; set; }
		public string Note { get; set; }
		public string Currency { get; set; }
		[JsonIgnore]
		public DateTime? DtUpdate { get; set; }
	}

	public class oEventRound
	{
		public int IdRound { get; set; }
		public string RoundName { get; set; }
		public int Distance { get; set; }
		public int Money { get; set; }
		public string Currency { get; set; }
		public List<oMatch> oMatches { get; set; } = new List<oMatch>();

	}
}
