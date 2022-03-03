﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Enumerations
{
	//TODO: Add constraint&Def to every table in DB
	public enum QuizStatus : short
	{
		OpenPredict = 0,
		MatchInProgress,
		ReOpenPredict,
		Done
	}

	public enum PredictStatus: short
	{
		Init = 0,
		Ended
	}

	public enum MatchStatus: short
	{
		NotStart = 0,
		Living,
		Ended
	}
}
