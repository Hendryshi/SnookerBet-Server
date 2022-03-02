using SnookerBet.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.JsonObjects
{
	public class oQuizPredict
	{
		public int IdEvent { get; set; }
		public string EventName { get; set; }
		public QuizStatus IdStatus { get; set; }
		public int IdGamer { get; set; }
		public List<oQuizRound> oQuizRounds { get; set; } = new List<oQuizRound>();
	}
}
