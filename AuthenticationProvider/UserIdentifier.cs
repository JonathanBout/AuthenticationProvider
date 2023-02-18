using LiteDB;

namespace JonathanBout.Authentication
{
	public abstract class UserIdentifier
	{
		[BsonId]
		public int UserId { get; set; }
		public abstract bool CompareString(string key);

		public static implicit operator int(UserIdentifier identifier)
		{
			return identifier.UserId;
		}
	}
}