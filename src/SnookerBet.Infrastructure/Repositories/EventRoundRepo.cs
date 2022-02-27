using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnookerBet.Core.Interfaces;
using SnookerBet.Core.Entities;
using SnookerBet.Infrastructure.DbContext;
using System.Transactions;

namespace SnookerBet.Infrastructure.Repositories
{
	public class EventRoundRepo : IEventRoundRepo
	{
		private readonly DapperContext db;

		public EventRoundRepo(DapperContext dbContext)
		{
			db = dbContext;
		}

		public EventRound Save(EventRound evtRound)
		{
			evtRound.DtUpdate = DateTime.Now;

			if(!db.UpdateEntity(evtRound))
				db.InsertEntity(evtRound);

			return evtRound;
		}

		public List<EventRound> SaveList(List<EventRound> evtRounds)
		{
			List<EventRound> lstEvtRounds = new List<EventRound>();
			using(var trans = new TransactionScope())
			{
				foreach(EventRound evtRound in evtRounds)
					lstEvtRounds.Add(Save(evtRound));

				trans.Complete();
			}
			return lstEvtRounds;
		}

		public List<EventRound> FindByEvent(int idEvent)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM S_EventRound WHERE idEvent = @idEvent");

			return db.Query<EventRound>(sql.ToString(), new { idEvent = idEvent });
		}

		public oEventRound GenerateOEventRound(EventRound evtRound)
		{
			return new oEventRound()
			{
				IdRound = evtRound.IdRound,
				RoundName = evtRound.RoundName,
				Distance = evtRound.Distance,
				Money = evtRound.Money,
				Currency = evtRound.Currency
			};
		}
	}
}
