using SnookerBet.Core.Entities;
using SnookerBet.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.JsonObjects
{
	public class oQuiz : BaseEntity
	{
		public int IdQuiz { get; set; }
		public int IdEvent { get; set; }
		public string Name { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public QuizStatus StQuiz { get; set; }
	}
}
