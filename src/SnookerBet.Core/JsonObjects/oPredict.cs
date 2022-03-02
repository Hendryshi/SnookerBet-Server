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
		public int IdPredict;
		public int IdGamer;
		public bool IsWinnerCorrect;
		public bool IsScoreCorrect;
		public PredictStatus predictStatus;
	}
}
