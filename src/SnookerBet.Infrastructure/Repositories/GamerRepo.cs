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
	public class GamerRepo : IGamerRepo
	{
		private readonly DapperContext db;

		public GamerRepo(DapperContext dbContext)
		{
			db = dbContext;
		}

		public GamerRepo Save(Quiz quiz)
		{
			quiz.DtUpdate = DateTime.Now;

			if(quiz.IdQuiz == 0)
				quiz.IdQuiz = (int)db.InsertEntity(quiz);
			else if(!db.UpdateEntity(quiz))
				throw new Exception($"Quiz not found in DB: {quiz.ToString()}");

			return quiz;
		}


		public Quiz FindByEvent(int idEvent)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM G_Quiz WHERE idEvent = @idEvent");

			return db.QuerySingleOrDefault<Quiz>(sql.ToString(), new { idEvent = idEvent });
		}

		
	}
}
