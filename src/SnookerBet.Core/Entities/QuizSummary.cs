using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using SnookerBet.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Entities
{
	[Table("G_QuizSummary")]
	public class QuizSummary : BaseEntity
	{
		[Key]
		public int IdSummary { get; set; }
		public int IdEvent { get; set; }
		public DateTime DtResult { get; set; }
		public string DescMatchSummary { get; set; }
		public string DescPointSummary { get; set; }
	}

}
