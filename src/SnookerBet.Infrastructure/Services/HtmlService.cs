using Newtonsoft.Json;
using SnookerBet.Core.Interfaces;
using SnookerBet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SnookerBet.Infrastructure.Services
{
	public class HtmlService
	{
		private readonly IAppLogger<HtmlService> _logger;

		public HtmlService(IAppLogger<HtmlService> logger)
		{
			_logger = logger;
		}

		public List<T> GetData<T>(string url, string urlParam = "")
		{
			List<T> result = null;
			HttpClient client = new HttpClient();
			try
			{
				_logger?.LogInformation(string.Format("Start getting data from API: {0}", url));
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				// List data response.
				HttpResponseMessage response = client.GetAsync(urlParam).Result;

				if(response.IsSuccessStatusCode)
				{
					result = JsonConvert.DeserializeObject<List<T>>(response.Content.ReadAsStringAsync().Result);
					_logger?.LogInformation(string.Format("Successfully getting data from the API. Count returned: {0}", result.Count()));
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

		public List<Match> GetMatchsInEvent(int idEvent = 0)
		{
			if(idEvent == 0)
				idEvent = 1140;

			_logger?.LogInformation(string.Format("API: Getting all the matchs in the event {0}", idEvent));
			List<Match> matchs = GetData<Match>(string.Format("http://api.snooker.org/?t=6&e={0}", idEvent));

			if(matchs != null && matchs.Count > 0)
				return matchs.OrderBy(c => c.IdRound).ThenBy(c => c.IdNumber).ToList();
			else
				throw new Exception("No match has been retrived from the API. Please Check !");
		}
	}
}
