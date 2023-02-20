namespace JonathanBout.Authentication
{
	public interface IAuthenticationSession
	{
		internal IUserIdentifier Identifier { get; set; }
		public int CurrentUserId { get; }
	}
}