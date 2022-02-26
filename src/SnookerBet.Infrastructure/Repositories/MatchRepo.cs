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
	public class MatchRepo : IMatchRepo
	{
		private readonly DapperContext db;

		public MatchRepo(DapperContext dbContext)
		{
			db = dbContext;
		}

		public Match Save(Match match)
		{
			match.DtUpdate = DateTime.Now;

			if(!db.UpdateEntity(match))
				db.InsertEntity(match);

			return match;
		}

		public List<Match> SaveList(List<Match> matches)
		{
			List<Match> lstMatches = new List<Match>();
			using(var trans = new TransactionScope())
			{
				foreach(Match match in matches)
					lstMatches.Add(Save(match));

				trans.Complete();
			}
			return lstMatches;
		}
	}
}
