using SnookerBet.Core.JsonObjects;

namespace SnookerBet.Core.Interfaces
{
	public interface IWechatService
	{
		string GetAccessToken();
		string GetUserOpenId(string code);
		void SendNotification(string token, WechatNotif wn);
	}
}