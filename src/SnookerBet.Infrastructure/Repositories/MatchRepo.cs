﻿using System;
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
	public class MatchRepo : IMatchRepo
	{
		private readonly DapperContext db;
		private readonly IPlayerRepo _playerRepo;
		public MatchRepo(DapperContext dbContext, IPlayerRepo playerRepo)
		{
			db = dbContext;
			_playerRepo = playerRepo;
		}

		public Match Save(Match match)
		{
			match.DtUpdate = DateTime.Now;

			if(!db.UpdateEntity(match))
				db.InsertEntity(match);

			return match;
		}

		public List<Match> SaveList(List<Match> matches, int idEvent)
		{
			List<Match> lstMatches = new List<Match>();
			using(var trans = new TransactionScope())
			{
				DeleteByEvent(idEvent);

				foreach(Match match in matches)
					lstMatches.Add(Save(match));

				trans.Complete();
			}
			return lstMatches;
		}

		public void DeleteByEvent(int idEvent)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"DELETE FROM S_Match WHERE idEvent = @idEvent");
			db.Execute(sql.ToString(), new { idEvent = idEvent });
		}

		public List<Match> FindByEvent(int idEvent)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM S_Match WHERE idEvent = @idEvent ORDER BY idEvent, idRound, number");

			List<Match> matches = db.Query<Match>(sql.ToString(), new { idEvent = idEvent });
			matches.ForEach(m => m.Player1 = _playerRepo.FindById(m.Player1Id));
			matches.ForEach(m => m.Player2 = _playerRepo.FindById(m.Player2Id));
			return matches;
		}

	}
}
