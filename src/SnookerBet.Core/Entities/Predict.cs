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
	[Table("G_Predict")]
	public class Predict : BaseEntity
	{
		[Key]
		public int IdPredict { get; set; }
		public int IdGamer { get; set; }
		public int IdEvent { get; set; }
		public int IdRound { get; set; }
		public int Number { get; set; }
		public int Player1Id { get; set; }
		public int Score1 { get; set; }
		public int Player2Id { get; set; }
		public int Score2 { get; set; }
		[Computed]
		public Player Player1 { get; set; }
		[Computed]
		public Player Player2 { get; set; }
		public int? WinnerId { get; set; }
		public int? Point { get; set; }
		public bool WinnerCorrect { get; set; } = false;
		public bool ScoreCorrect { get; set; } = false;
		public DateTime? DtResult { get; set; }
		public PredictStatus idStatus { get; set; } = PredictStatus.Init;
		public DateTime? DtUpdate { get; set; }
	}

}
