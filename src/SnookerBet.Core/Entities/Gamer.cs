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
	[Table("G_Gamer")]
	public class Gamer : BaseEntity
	{
		[Key]
		public int IdGamer { get; set; }
		public int IdQuiz { get; set; }
		public string wechatName { get; set; }
		public string gamerName { get; set; }
		public int totalScore { get; set; }
		public DateTime? DtUpdate { get; set; }
	}

}
