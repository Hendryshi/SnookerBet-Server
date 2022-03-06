using SnookerBet.Core.Entities;
using System.Collections.Generic;

namespace SnookerBet.Core.Interfaces
{
	public interface IPredictRepo
	{
		List<Predict> FindByMatch(int idEvent, int idRound, int idNumber);
		Predict FindByMatchAndGamer(int idGamer, int idEvent, int idRound, int idNumber);
		List<Predict> LoadPredictsByEventAndGamer(int idEvent, int idGamer);
		Predict Save(Predict predict);
		List<Predict> SaveList(List<Predict> predicts);
	}
}