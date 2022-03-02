using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Entities
{
	[Table("S_Event")]
	public class Event: BaseEntity
	{
		[ExplicitKey]
		[JsonProperty("ID")]
		public int IdEvent { get; set; }
		public string Name { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public int? Season { get; set; }
		[JsonProperty("Type")]
		public string TyEvent { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public bool AllRoundsAdded { get; set; } = false;
		public int? NumCompetitors { get; set; }
		public int? NumUpComing { get; set; }
		public int? NumActive { get; set; }
		public int? numResults { get; set; }
		public string note { get; set; }
		public string commonNote { get; set; }
		[JsonIgnore]
		public DateTime? DtUpdate { get; set; }

		[JsonIgnore]
		[Computed]
		public List<EventRound> EventRounds { get; set; } = new List<EventRound>();
		[JsonIgnore]
		[Computed]
		public List<Match> EventMatches { get; set; } = new List<Match>();
	}

	
}
