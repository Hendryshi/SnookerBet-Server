using SnookerBet.Core.Entities;
using SnookerBet.Core.JsonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Interfaces
{
	public interface IMatchRepo
	{
		Match Save(Match match);
		List<Match> SaveList(List<Match> matches, int idEvent);
		List<Match> FindByEvent(int idEvent);
		oMatch GenerateOMatch(Match match);
	}
}
