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
	public class PlayerRepo : IPlayerRepo
	{
		private readonly DapperContext db;

		public PlayerRepo(DapperContext dbContext)
		{
			db = dbContext;
		}

		public Player Save(Player player)
		{
			player.DtUpdate = DateTime.Now;

			if(!db.UpdateEntity(player))
				db.InsertEntity(player);

			return player;
		}

		public List<Player> SaveList(List<Player> players)
		{
			List<Player> lstPlayers = new List<Player>();
			using(var trans = new TransactionScope())
			{
				foreach(Player player in players)
					lstPlayers.Add(Save(player));

				trans.Complete();
			}
			return lstPlayers;
		}

		public Player FindById(int idPlayer)
		{
			return db.GetEntityById<Player>(idPlayer);
		}

		public oPlayer GenerateOPlayer(Player player)
		{
			return new oPlayer()
			{
				IdPlayer = player.IdPlayer,
				Name = player.ChineseName ?? (player.LastName ?? player.FirstName),
				Photo = player.Photo,
				Rank = player.SeasonRank
			};
		}

		public oPlayer GenerateOPlayer(int idPlayer)
		{
			oPlayer output = null;
			Player pl = FindById(idPlayer);
			if(pl != null)
				output = GenerateOPlayer(pl);

			return output;
		}
	}
}
