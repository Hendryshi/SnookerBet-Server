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
	public class PredictRepo : IPredictRepo
	{
		private readonly DapperContext db;
		private readonly IPlayerRepo _playerRepo;

		public PredictRepo(DapperContext dbContext, IPlayerRepo playerRepo)
		{
			db = dbContext;
			_playerRepo = playerRepo;
		}

		public Predict Save(Predict predict)
		{
			predict.DtUpdate = DateTime.Now;

			if(predict.IdPredict == 0)
				predict.IdPredict = (int)db.InsertEntity(predict);
			else if(!db.UpdateEntity(predict))
				throw new Exception($"Predict not found in DB: {predict.ToString()}");

			return predict;
		}

		public List<Predict> SaveList(List<Predict> predicts)
		{
			List<Predict> lstPredict = new List<Predict>();
			using(var trans = new TransactionScope())
			{
				foreach(Predict p in predicts)
					lstPredict.Add(Save(p));

				trans.Complete();
			}
			return lstPredict;
		}

		public List<Predict> LoadPredictsByEventAndGamer(int idEvent, int idGamer)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM G_Predict WHERE idEvent = @idEvent AND idGamer = @idGamer");

			List<Predict> predicts = db.Query<Predict>(sql.ToString(), new { idEvent = idEvent, idGamer = idGamer });

			predicts.ForEach(p => p.Player1 = _playerRepo.FindById(p.Player1Id));
			predicts.ForEach(p => p.Player2 = _playerRepo.FindById(p.Player2Id));

			return predicts;
		}

		public Predict FindByMatchAndGamer(int idGamer, int idEvent, int idRound, int idNumber)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM G_Predict WHERE idGamer = @idGamer AND idEvent = @idEvent AND idRound = @idRound AND number = @number");

			Predict predict = db.QuerySingleOrDefault<Predict>(sql.ToString(), new { idGamer = idGamer, idEvent = idEvent, idRound = idRound, number = idNumber });

			if(predict != null)
			{
				predict.Player1 = _playerRepo.FindById(predict.Player1Id);
				predict.Player2 = _playerRepo.FindById(predict.Player2Id);
			}
			
			return predict;
		}

		public List<Predict> FindByMatch(int idEvent, int idRound, int idNumber)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM G_Predict WHERE idEvent = @idEvent AND idRound = @idRound AND number = @number");

			List<Predict> predicts = db.Query<Predict>(sql.ToString(), new { idEvent = idEvent, idRound = idRound, number = idNumber });

			predicts.ForEach(p => p.Player1 = _playerRepo.FindById(p.Player1Id));
			predicts.ForEach(p => p.Player2 = _playerRepo.FindById(p.Player2Id));

			return predicts;
		}
	}
}
