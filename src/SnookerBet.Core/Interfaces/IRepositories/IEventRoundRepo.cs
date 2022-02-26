using SnookerBet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Interfaces
{
	public interface IEventRoundRepo
	{
		EventRound Save(EventRound evtRound);
		List<EventRound> SaveList(List<EventRound> evtRounds);
	}
}
