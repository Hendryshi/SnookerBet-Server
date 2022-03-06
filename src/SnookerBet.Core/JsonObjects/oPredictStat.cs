using SnookerBet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.JsonObjects
{
	public class oPredictStat : BaseEntity
	{
		public int IdGamer { get; set; }
		public int IdEvent { get; set; }
		public string GamerName { get; set; }
		public int TotalPredict { get; set; }
		public int NbrWinnerCorrect { get; set; }
		public int NbrScoreCorrect { get; set; }
		public int TotalPoint { get; set; }

	}

	public class oPredictByDay : BaseEntity
	{
		public DateTime DtResult { get; set; }
		public int Point { get; set; }
		public int CumulPoint { get; set; }
	}

	public class oPredictGamerTrend : BaseEntity
	{
		public int IdGamer { get; set; }
		public string GamerName { get; set; }
		public int IdEvent { get; set; }
		public List<oPredictByDay> oPredictByDays { get; set; } = new List<oPredictByDay>();
	}
}
