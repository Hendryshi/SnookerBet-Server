using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Entities
{
	[Table("S_Match")]
	public class Match : BaseEntity
	{
		[ExplicitKey]
		[JsonProperty("ID")]
		public int IdMatch { get; set; }
		[JsonProperty("EventID")]
		public int IdEvent { get; set; }
		[JsonProperty("Round")]
		public int IdRound { get; set; }
		public int Number { get; set; }
		public int Player1Id { get; set; }
		public int? Score1 { get; set; }
		public int Player2Id { get; set; }
		public int? Score2 { get; set; }
		public int? WinnerId { get; set; }
		[Computed]
		public Player Player1 { get; set; }
		[Computed]
		public Player Player2 { get; set; }
		public bool Unfinished { get; set; } = true;
		public bool OnBreak { get; set; } = false;
		public DateTime? InitDate { get; set; }
		public DateTime? ModDate { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime? ScheduledDate { get; set; }
		public string note { get; set; }
		public string extendedNote { get; set; }
		[JsonIgnore]
		public DateTime? DtUpdate { get; set; }
	}
}

