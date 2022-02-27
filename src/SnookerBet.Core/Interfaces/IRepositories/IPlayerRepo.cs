﻿using SnookerBet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Interfaces
{
	public interface IPlayerRepo
	{
		Player FindById(int idPlayer);
		Player Save(Player player);
		List<Player> SaveList(List<Player> players);
		oPlayer GenerateOPlayer(Player player);
		oPlayer GenerateOPlayer(int idPlayer);
	}
}
