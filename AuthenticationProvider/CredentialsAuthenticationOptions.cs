using System.Text.RegularExpressions;

namespace JonathanBout.Authentication
{
	public partial class CredentialsAuthenticationOptions
		: AuthenticationOptions<CredentialsAuthenticationOptions>
	{
		/// <summary>
		/// The Regular Expression for the username. Default 
		/// matches: 'a'-'z', 'A'-'Z', '0'-'9', '-' and '_', with a minimum size of 
		/// 3 and a maximum size of 25.
		/// </summary>
		public Regex UsernameRegex { get; set; } = UsernameRegexFunc();

		/// <summary>
		/// The Regular Expression for the password. Default 
		/// matches: a password that contains at least A-Z, a-z, 0-9 and any
		/// character of '-_!@#$%^&amp;*<>,.;:'=+',
		/// with a minimum size of 5.
		/// </summary>
		public Regex PasswordRegex { get; set; } = PasswordRegexFunc();


		[GeneratedRegex("^(?:\\w|[-]){3,25}$")]
		private static partial Regex UsernameRegexFunc();

		[GeneratedRegex(
			/*language=regex*/
			"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9-_!@#$%^&*<>,.;:'=+])" +
			"[a-zA-Z0-9-_!@#$%^&*<>,.;:']{5,}$")]
		private static partial Regex PasswordRegexFunc();
	}
}