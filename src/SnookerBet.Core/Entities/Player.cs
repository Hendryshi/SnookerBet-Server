using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Entities
{
	[Table("S_Player")]
	public class Player: BaseEntity
	{
		[ExplicitKey]
		[JsonProperty("ID")]
		public int IdPlayer { get; set; }
		public short Type { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string ShortName { get; set; }
		[JsonIgnore]
		[Computed]
		public string ChineseName { get; set; }
		public string Nationality { get; set; }
		public string Sex { get; set; }
		public DateTime? Born { get; set; }
		public int? FirstSeasonAsPro { get; set; }
		public int? LastSeasonAsPro { get; set; }
		[JsonIgnore]
		public int? SeasonRank { get; set; }
		[JsonIgnore]
		[Computed]
		public string Photo { get; set; }
		[JsonIgnore]
		public DateTime DtUpdate { get; set; }
	}

	public class Rank: BaseEntity
	{
		public int PlayerId { get; set; }
		public int Position { get; set; }
		public int Sum { get; set; }
	}

	public class oPlayer
	{
		public int IdPlayer { get; set; }
		public string Name { get; set; }
		public int? Rank { get; set; }
		public string Photo { get; set; }
	}
}
