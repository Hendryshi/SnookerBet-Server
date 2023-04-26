using Newtonsoft.Json;
using SnookerBet.Core.Interfaces;
using SnookerBet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using SnookerBet.Core.Settings;
using Microsoft.Extensions.Options;

namespace SnookerBet.Core.Services
{
	public class ExternalDataService : IExternalDataService
	{
		private readonly SnookerOrgSettings _snookerOrgSettings;
		private readonly IAppLogger<ExternalDataService> _logger;

		public ExternalDataService(IAppLogger<ExternalDataService> logger, IOptionsSnapshot<SnookerOrgSettings> snookerOrgSettings)
		{
			_logger = logger;
			_snookerOrgSettings = snookerOrgSettings.Value;
		}

		private List<T> GetData<T>(string url, string urlParam = "")
		{
			List<T> result = null;
			HttpClient client = new HttpClient();
			try
			{
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Add("X-Requested-by", "YejiaShi");
				// List data response.
				HttpResponseMessage response = client.GetAsync(urlParam).Result;

				if(response.IsSuccessStatusCode)
				{
					result = JsonConvert.DeserializeObject<List<T>>(response.Content.ReadAsStringAsync().Result);
					_logger?.LogInformation(string.Format("Successfully getting data from the API"));
				}
				else
					_logger?.LogError("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

				return result;
			}
			catch(Exception ex)
			{
				_logger?.LogError(ex, string.Format("Error when getting data from url: [{0}], urlParam: [{1}]", url, urlParam));
				return result;
			}
			finally
			{
				client.Dispose();
			}
		}

		public Event GetEvent(int idEvent)
		{
			_logger?.LogInformation(string.Format("External API: Getting event info [idEvent={0}]", idEvent));
			List<Event> events = GetData<Event>(string.Format(_snookerOrgSettings.EventUrl, idEvent));

			return events?.FirstOrDefault();
		}

		public Player GetPlayer(int idPlayer)
		{
			_logger?.LogInformation(string.Format("External API: Getting player info [idPlayer={0}]", idPlayer));
			List<Player> pl = GetData<Player>(string.Format(_snookerOrgSettings.PlayerUrl, idPlayer));

			return pl?.FirstOrDefault();
		}

		public List<Event> GetEventsInSeason(int season = 0)
		{
			if(season == 0) season = _snookerOrgSettings.Season;
			_logger?.LogInformation(string.Format("External API: Getting events info in season [{0}]", season));
			return GetData<Event>(string.Format(_snookerOrgSettings.EventsInSeasonUrl, season));
		}

		public List<Player> GetPlayersInSeason(int season = 0)
		{
			if(season == 0) season = _snookerOrgSettings.Season;
			_logger?.LogInformation(string.Format("External API: Getting players info in season [{0}]", season));
			return GetData<Player>(string.Format(_snookerOrgSettings.PlayersInSeasonUrl, season));
		}

		public List<Player> GetPlayersInEvent(int idEvent = 0)
		{
			_logger?.LogInformation(string.Format("External API: Getting players info in event[id={0}]", idEvent));
			return GetData<Player>(string.Format(_snookerOrgSettings.PlayersInEventUrl, idEvent));
		}

		public List<Rank> GetSeasonRankInSeason(int season = 0)
		{
			if(season == 0) season = _snookerOrgSettings.Season;
			_logger?.LogInformation(string.Format("External API: Getting ranking info in season [{0}]", season));
			return GetData<Rank>(string.Format(_snookerOrgSettings.RankingUrl, season));
		}

		public List<EventRound> GetRoundsInEvent(int idEvent)
		{
			_logger?.LogInformation(string.Format("External API: Getting all rounds info in event [{0}]", idEvent));
			return GetData<EventRound>(string.Format(_snookerOrgSettings.RoundInfoUrl, idEvent));
		}

		public List<Match> GetMatchesInEvent(int idEvent)
		{
			_logger?.LogInformation(string.Format("External API: Getting all matches info in event [{0}]", idEvent));
			return GetData<Match>(string.Format(_snookerOrgSettings.MatchsInEvtUrl, idEvent));
		}
	}
}
