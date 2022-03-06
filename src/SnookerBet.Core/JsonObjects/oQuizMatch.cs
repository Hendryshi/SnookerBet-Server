using SnookerBet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.JsonObjects
{
	public class oQuizMatch	: BaseEntity
	{
		public oMatch OMatch { get; set; }
		public List<oPredict> oPredicts { get; set; } = new List<oPredict>();
	}
}
