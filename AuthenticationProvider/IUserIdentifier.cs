using LiteDB;

namespace JonathanBout.Authentication
{
	internal interface IUserIdentifier<TSelf> : IUserIdentifier
		where TSelf : IUserIdentifier<TSelf>
	{
		internal static abstract (string key, TSelf identifier) Create(int userId);
	}

	public interface IUserIdentifier
	{
		[BsonId]
		public int UserId { get; set; }
	}
}