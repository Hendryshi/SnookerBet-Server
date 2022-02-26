using Newtonsoft.Json;

namespace SnookerBet.Core.Entities
{
	public abstract class BaseEntity
	{
		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}
