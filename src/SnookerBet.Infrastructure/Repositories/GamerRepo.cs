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
		private readonly IPredictRepo _predictRepo;

		public GamerRepo(DapperContext dbContext, IPredictRepo predictRepo)
		{
			db = dbContext;
			_predictRepo = predictRepo;
		}

		public Gamer Save(Gamer gamer, bool updatePredict = false)
		{
			gamer.DtUpdate = DateTime.Now;
			using(var trans = new TransactionScope())
			{

				if(gamer.IdGamer == 0)
					gamer.IdGamer = (int)db.InsertEntity(gamer);
				else if(!db.UpdateEntity(gamer))
					throw new Exception($"Gamer not found in DB: {gamer.ToString()}");

				if(updatePredict)
				{
					gamer.predicts.ForEach(p => p.IdGamer = gamer.IdGamer);
					_predictRepo.SaveList(gamer.predicts);
				}

				trans.Complete();
			}

			return gamer;
		}

		public List<Gamer> SaveList(List<Gamer> gamers, bool updatePredict = false)
		{
			List<Gamer> lstGamers = new List<Gamer>();
			using(var trans = new TransactionScope())
			{
				foreach(Gamer g in gamers)
					lstGamers.Add(Save(g, updatePredict));

				trans.Complete();
			}
			return lstGamers;
		}

		public Gamer FindById(int idGamer)
		{
			return db.GetEntityById<Gamer>(idGamer);
		}

		public Gamer FindByEventAndName(int idEvent, string wechatName, bool loadPredict = true)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM G_Gamer WHERE idEvent = @idEvent AND wechatName = @wechatName");

			Gamer gamer = db.QuerySingleOrDefault<Gamer>(sql.ToString(), new { idEvent = idEvent, wechatName = wechatName });

			if(gamer != null && loadPredict)
				gamer.predicts = _predictRepo.LoadPredictsByEventAndGamer(idEvent, gamer.IdGamer);
				
			return gamer;
		}

		public List<Gamer> LoadAllByEvent(int idEvent, bool loadPredict = true)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM G_Gamer WHERE idEvent = @idEvent");

			List<Gamer> gamers = db.Query<Gamer>(sql.ToString(), new { idEvent = idEvent});

			if(loadPredict)
				gamers.ForEach(g => g.predicts = _predictRepo.LoadPredictsByEventAndGamer(idEvent, g.IdGamer));

			return gamers;
		}
	}
}
