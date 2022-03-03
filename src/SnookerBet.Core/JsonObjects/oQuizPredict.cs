using SnookerBet.Core.Entities;
using SnookerBet.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.JsonObjects
{
	public class oQuizPredict : BaseEntity
	{
		public int IdEvent { get; set; }
		public string EventName { get; set; }
		public bool ReadOnly { get; set; } = false;
		public oGamer oGamer { get; set; } = new oGamer();
		public List<oQuizRound> oQuizRounds { get; set; } = new List<oQuizRound>();
	}
}
