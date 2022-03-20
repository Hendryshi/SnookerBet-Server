using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnookerBet.Core.Interfaces;
using SnookerBet.Core.Entities;
using SnookerBet.Infrastructure.DbContext;
using System.Transactions;
using SnookerBet.Core.Enumerations;
using SnookerBet.Core.JsonObjects;

namespace SnookerBet.Infrastructure.Repositories
{
	public class QuizRepo : IQuizRepo
	{
		private readonly DapperContext db;

		public QuizRepo(DapperContext dbContext)
		{
			db = dbContext;
		}

		public Quiz Save(Quiz quiz)
		{
			quiz.DtUpdate = DateTime.Now;

			if(quiz.IdQuiz == 0)
				quiz.IdQuiz = (int)db.InsertEntity(quiz);
			else if(!db.UpdateEntity(quiz))
				throw new Exception($"Quiz not found in DB: {quiz.ToString()}");

			return quiz;
		}

		public QuizSummary SaveSummary(QuizSummary summary)
		{
			if(summary.IdSummary == 0)
				summary.IdSummary = (int)db.InsertEntity(summary);
			else if(!db.UpdateEntity(summary))
				throw new Exception($"QuizSummary not found in DB: {summary.ToString()}");

			return summary;
		}

		public Quiz FindByEvent(int idEvent)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM G_Quiz WHERE idEvent = @idEvent");

			return db.QuerySingleOrDefault<Quiz>(sql.ToString(), new { idEvent = idEvent });
		}

		public List<Quiz> FindByStatus(List<QuizStatus> quizStatuses)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM G_Quiz WHERE idStatus IN @status");

			return db.Query<Quiz>(sql.ToString(), new { status = quizStatuses });
		}

		public List<oPredictStat> GetPredictSummary(int idEvent, int idGamer = 0)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM vPredictSummary WHERE idEvent = @idEvent");

			if(idGamer != 0)
				sql.AppendLine(" AND idGamer = @idGamer");

			return db.Query<oPredictStat>(sql.ToString(), new { idEvent = idEvent, idGamer = idGamer });
		}

		public List<oPredictByDay> GetPredictPointByDay(int idEvent, int idGamer)
		{
			var sql = new StringBuilder();
			sql.AppendFormat("EXECUTE GetGamerTrendingByDay @idEvent={0}, @idGamer={1}", idEvent, idGamer);

			return db.Query<oPredictByDay>(sql.ToString());
		}

		public List<QuizSummary> GetQuizSummary(int idEvent)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM G_QuizSummary WHERE idEvent = @idEvent ORDER BY dtResult DESC");

			return db.Query<QuizSummary>(sql.ToString(), new { idEvent = idEvent });
		}
	}
}
