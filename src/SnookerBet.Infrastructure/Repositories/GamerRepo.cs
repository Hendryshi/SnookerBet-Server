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

		public Gamer Save(Gamer gamer)
		{
			gamer.DtUpdate = DateTime.Now;

			if(gamer.IdGamer == 0)
				gamer.IdGamer = (int)db.InsertEntity(gamer);
			else if(!db.UpdateEntity(gamer))
				throw new Exception($"Gamer not found in DB: {gamer.ToString()}");

			return gamer;
		}


		public Gamer FindByEvent(int idEvent, string wechatName, bool loadPredict = true)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM G_Gamer WHERE idEvent = @idEvent AND wechatName = @wechatName");

			Gamer gamer = db.QuerySingleOrDefault<Gamer>(sql.ToString(), new { idEvent = idEvent, wechatName = wechatName });

			if(gamer != null && loadPredict)
				gamer.predicts = _predictRepo.LoadPredictsByEventAndGamer(idEvent, gamer.IdGamer);
				
			return gamer;
		}
	}
}
