using SnookerBet.Core.Entities;
using System.Collections.Generic;

namespace SnookerBet.Core.Interfaces
{
	public interface IPredictRepo
	{
		List<Predict> LoadPredictsByEventAndGamer(int idEvent, int idGamer);
		Predict Save(Predict predict);
	}
}