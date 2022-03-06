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
		public int IdEvent { get; set; }
		public string WechatName { get; set; }
		public string GamerName { get; set; }
		public int TotalScore { get; set; }
		public bool ChangePredict { get; set; } = false;
		public DateTime? DtUpdate { get; set; }
		[Computed]
		public List<Predict> predicts { get; set; } = new List<Predict>();
	}

}
