using SnookerBet.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.JsonObjects
{
	public class oPredict : oMatch
	{
		public int IdPredict { get; set; }
		public int IdGamer { get; set; }
		public string GamerName { get; set; }
		public bool IsWinnerCorrect { get; set; } = false;
		public bool IsScoreCorrect { get; set; } = false;
		public int? Point { get; set; }
		public PredictStatus PredictStatus { get; set; } = PredictStatus.Init;

		public oPredict(oMatch m) : base(m) { }

		public oPredict()
		{
		}
	}
}
