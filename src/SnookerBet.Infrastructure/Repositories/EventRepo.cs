using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnookerBet.Core.Interfaces;
using SnookerBet.Core.Entities;
using SnookerBet.Infrastructure.DbContext;
using System.Transactions;
using SnookerBet.Core.JsonObjects;

namespace SnookerBet.Infrastructure.Repositories
{
	public class EventRepo : IEventRepo
	{
		private readonly DapperContext db;
		private readonly IEventRoundRepo _eventRoundRepo;
		private readonly IMatchRepo _matchRepo;

		public EventRepo(DapperContext dbContext, IEventRoundRepo eventRoundRepo, IMatchRepo matchRepo)
		{
			db = dbContext;
			_eventRoundRepo = eventRoundRepo;
			_matchRepo = matchRepo;
		}

		public Event Save(Event evt, bool onlyEvent = true)
		{
			evt.DtUpdate = DateTime.Now;

			using(var trans = new TransactionScope())
			{
				if(!db.UpdateEntity(evt))
					db.InsertEntity(evt);

				if(!onlyEvent)
				{
					if(evt.EventRounds.Count > 0)
						_eventRoundRepo.SaveList(evt.EventRounds);

					if(evt.EventMatches.Count > 0)
						_matchRepo.SaveList(evt.EventMatches, evt.IdEvent);
				}

				trans.Complete();
			}

			return evt;
		}

		public List<Event> SaveList(List<Event> events)
		{
			List<Event> lstEvents = new List<Event>();
			using(var trans = new TransactionScope())
			{
				foreach(Event _event in events)
					lstEvents.Add(Save(_event));

				trans.Complete();
			}
			return lstEvents;
		}

		public Event FindById(int idEvent, bool loadMatch = true)
		{
			Event evt = db.GetEntityById<Event>(idEvent);
			if(loadMatch && evt != null)
			{
				evt.EventRounds = _eventRoundRepo.FindByEvent(idEvent);
				evt.EventMatches = _matchRepo.FindByEvent(idEvent);
			}
			return evt;
		}

	}
}
