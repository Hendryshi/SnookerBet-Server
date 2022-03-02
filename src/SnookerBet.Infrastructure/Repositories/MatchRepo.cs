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

			return db.Query<Match>(sql.ToString(), new { idEvent = idEvent });
		}

		public oMatch GenerateOMatch(Match match)
		{
			return new oMatch() {
				IdMatch = match.IdMatch,
				IdRound = match.IdRound,
				IdEvent = match.IdEvent,
				Number = match.Number,
				Player1 = _playerRepo.GenerateOPlayer(match.Player1Id),
				Score1 = match.Score1.Value,
				Player2 = _playerRepo.GenerateOPlayer(match.Player2Id),
				Score2 = match.Score2.Value,
				WinnerId = match.WinnerId.Value,
				IdStatus = (short)(match.StartDate == null ? 0 : (match.EndDate == null ? 1 : 2)), //0: NotStart 1: Living 2: End
				ScheduledDate = match.ScheduledDate,
				note = match.note + " " + match.extendedNote
			};
		}
	}
}
