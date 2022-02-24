using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Core.Entities
{
	public class Match
	{
		[JsonProperty("ID")]
		public int IdMatch;
		[JsonProperty("EventID")]
		public int IdEvent;
		[JsonProperty("Round")]
		public int IdRound;
		[JsonProperty("Number")]
		public int IdNumber;
		public int WorldSnookerID;
		[JsonProperty("Player1ID")]
		public int IdPlayer1;
		public int Score1;
		[JsonProperty("Player2ID")]
		public int IdPlayer2;
		public int Score2;
		[JsonProperty("WinnerID")]
		public int IdWinner;
		[JsonProperty("Unfinished")]
		public bool UnFinished;
		public int Status; //0: unfinised 1: ongoing 2:finised
		[JsonProperty("StartDate")]
		public DateTime? DtStart;
		[JsonProperty("EndDate")]
		public DateTime? DtEnd;
		[JsonProperty("ScheduledDate")]
		public DateTime? DtSchedule;
		public string Sessions;
		private string score;
		private string player1Name;
		private string player2Name;
		private string winnerName;
		public string Note;

	}
}
