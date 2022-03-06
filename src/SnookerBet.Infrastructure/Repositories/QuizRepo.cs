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

		public List<oPredictStat> GetPredictSummary(int idEvent)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM vPredictSummary WHERE idEvent = @idEvent");

			return db.Query<oPredictStat>(sql.ToString(), new { idEvent = idEvent });
		}

		public List<oPredictByDay> GetPredictPointByDay(int idEvent, int idGamer)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT CAST(t.dtResult AS date) dtResult, ISNULL(point, 0) point,");
			sql.AppendLine("SUM(ISNULL(point, 0)) OVER(ORDER BY CAST(t.dtResult AS date) ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS cumulPoint");
			sql.AppendLine("FROM (");
			sql.AppendLine("	SELECT distinct CAST(dtResult as date) dtResult from G_Predict");
			sql.AppendLine("	where idEvent = @idEvent AND dtResult is not null");
			sql.AppendLine(") t");
			sql.AppendLine("LEFT JOIN G_Predict p ON CAST(p.dtResult as date) = t.dtResult AND idEvent = @idEvent AND idGamer = @idGamer");
			sql.AppendLine("ORDER BY CAST(t.dtResult AS date)");

			return db.Query<oPredictByDay>(sql.ToString(), new { idEvent = idEvent, idGamer = idGamer });
		}
	}
}
