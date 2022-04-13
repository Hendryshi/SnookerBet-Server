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
using SnookerBet.Core.JsonObjects;
using System.Text;

namespace SnookerBet.Core.Services
{
	public class WechatService : IWechatService
	{
		private readonly WechatSettings _wechatSettings;
		private readonly IAppLogger<WechatService> _logger;

		public WechatService(IAppLogger<WechatService> logger, IOptionsSnapshot<WechatSettings> wechatSettings)
		{
			_logger = logger;
			_wechatSettings = wechatSettings.Value;
		}

		public string GetUserOpenId(string code)
		{
			string openId = string.Empty;
			using(HttpClient client = new HttpClient())
			{
				try
				{
					client.BaseAddress = new Uri("https://api.weixin.qq.com/sns/jscode2session");
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					string urlParam = string.Format("?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", _wechatSettings.AppId, _wechatSettings.AppSecret, code);

					HttpResponseMessage response = client.GetAsync(urlParam).Result;

					if(response.IsSuccessStatusCode)
					{
						dynamic result = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
						if(result.openid != null)
							openId = result.openid.Value;
					}
				}
				catch(Exception e)
				{
					_logger.LogError($"Exception when requesting openid for code {code}", e);
				}

				return openId;
			}
		}

		public string GetAccessToken()
		{
			string token = string.Empty;
			using(HttpClient client = new HttpClient())
			{
				client.BaseAddress = new Uri("https://api.weixin.qq.com/cgi-bin/token");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				string urlParam = string.Format("?grant_type=client_credential&appid={0}&secret={1}", _wechatSettings.AppId, _wechatSettings.AppSecret);

				HttpResponseMessage response = client.GetAsync(urlParam).Result;

				if(response.IsSuccessStatusCode)
				{
					dynamic result = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
					if(result.access_token != null)
						token = result.access_token.Value;
				}

				return token;
			}
		}

		public void SendNotification(string token, WechatNotif wn)
		{
			wn.miniprogram_state = _wechatSettings.State;
			wn.template_id = _wechatSettings.TemplateId;
			
			using(HttpClient client = new HttpClient())
			{
				var json = JsonConvert.SerializeObject(wn);
				var jsonData = new StringContent(json, Encoding.UTF8, "application/json");

				client.BaseAddress = new Uri("https://api.weixin.qq.com/cgi-bin/message/subscribe/send");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				string urlParam = string.Format("?access_token={0}", token);

				var response = client.PostAsync(urlParam, jsonData).Result;
				_logger.LogInformation(response.Content.ReadAsStringAsync().Result);
			}
		}
	}
}
