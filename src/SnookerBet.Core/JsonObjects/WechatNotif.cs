using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.JsonObjects
{
	public class WechatNotif
	{
		public string miniprogram_state { get; set; }
		public string page { get; set; }
		public string template_id { get; set; }
		public string touser { get; set; }
		public Object data { get; set; }
	}

}
