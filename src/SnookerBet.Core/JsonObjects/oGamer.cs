using SnookerBet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnookerBet.Core.JsonObjects
{
	public class oGamer : BaseEntity
	{
		public int IdGamer { get; set; }
		public int IdEvent { get; set; }
		public string wechatName { get; set; }
		public string gamerName { get; set; }

		[JsonConstructor]
		public oGamer() { }
	}
}
