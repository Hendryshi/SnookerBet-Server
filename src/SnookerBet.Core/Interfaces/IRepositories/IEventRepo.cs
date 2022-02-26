using SnookerBet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Interfaces
{
	public interface IEventRepo
	{
		Event FindById(int idEvent);
		Event Save(Event evt, bool onlyEvent = true);
		List<Event> SaveList(List<Event> events);
	}
}
