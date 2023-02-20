namespace JonathanBout.Authentication
{
	public class KeyAuthenticationOptions
		: AuthenticationOptions<KeyAuthenticationOptions>
	{
		/// <summary>
		/// The size of the key to generate. Default is 64.
		/// </summary>
		public int KeySize { get; set; } = 64;
		public override string CookieName { get; set; } = "AuthKey";

		public KeyAuthenticationOptions() { }
	}
}