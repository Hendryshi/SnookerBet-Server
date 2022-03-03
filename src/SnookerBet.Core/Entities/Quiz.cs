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
	[Table("G_Quiz")]
	public class Quiz : BaseEntity
	{
		[Key]
		public int IdQuiz { get; set; }
		public int IdEvent { get; set; }
		public DateTime? DtStart { get; set; }
		public DateTime? DtEnd { get; set; }
		public QuizStatus IdStatus { get; set; } = QuizStatus.OpenPredict;
		public DateTime? DtUpdate { get; set; }
	}
}
