namespace JonathanBout.Authentication
{
	public interface IAuthenticationSession
	{
		internal UserIdentifier Identifier { get; set; }
		public int CurrentUserId { get; }
	}
}