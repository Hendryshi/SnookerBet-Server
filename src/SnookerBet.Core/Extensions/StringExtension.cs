using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Extensions
{
	public static class StringExtension
	{
		public static string Translate(this string str, string type)
		{
			switch(type)
			{
				case "Round":
					return TranslateRound(str);
				case "Event":
					return TranslateEvent(str);
				default:
					return str;
			}
		}

		private static string TranslateRound(string str)
		{
			if(str == "Round 1")
				str = "第一轮";
			else if(str == "Round 2")
				str = "第二轮";
			else if(str == "Round 3")
				str = "第三轮";
			else if(str == "Round 4")
				str = "第四轮";
			else if(str == "Round 5")
				str = "第五轮";
			else if(str == "Round 6")
				str = "第六轮";
			else if(str == "Quarterfinals")
				str = "1/4 决赛";
			else if(str == "Semifinals")
				str = "半决赛";
			else if(str == "Final")
				str = "决赛";

			return str;
		}

		private static string TranslateEvent(string str)
		{
			if(str == "Gibraltar Open")
				str = "直布罗陀公开赛";
			else if(str == "Tour Championship")
				str = "巡回锦标赛";
			else if(str == "World Championship")
				str = "斯诺克世锦赛";
			else if(str == "Turkish Masters")
				str = "土耳其大师赛";
			else if(str == "Welsh Open")
				str = "威尔士公开赛";
			else if(str == "European Master")
				str = "欧洲大师赛";
			else if(str == "British Open")
				str = "英国公开赛";
			else if(str == "English Open")
				str = "英格兰公开赛";
			else if(str == "Champion of Champions")
				str = "斯诺克冠中冠";
			else if(str == "UK Championship")
				str = "斯诺克英锦赛";

			return str;
		}
	}
}
