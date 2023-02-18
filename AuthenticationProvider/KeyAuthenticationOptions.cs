using System.Reflection;

namespace JonathanBout.Authentication
{
	public class KeyAuthenticationOptions
	{
		internal static KeyAuthenticationOptions Instance { get; } = new();
		/// <summary>
		/// The size of the key to generate. Default is 64.
		/// </summary>
		public int KeySize { get; set; } = 64;
		/// <summary>
		/// The name of the cookie. Default is <c>"AuthKey"</c>.
		/// </summary>
		public string CookieName { get; set; } = "AuthKey";
		/// <summary>
		/// The base path for the endpoints. Here, the /Key and /Login endpoints will be created. Default is <c>"/Authentication"</c>.
		/// </summary>
		public string BasePath { get; set; } = "/Authentication";

		/// <summary>
		/// The url to the database file. Default is <c>"C:\Users\Public\Documents\[Assembly Name]\authentication.db"</c>.
		/// </summary>
		public string DatabasePath { get; set; } = "C:\\Users\\Public\\Documents\\"
			+ (Assembly.GetEntryAssembly()?.GetName().Name??Assembly.GetExecutingAssembly().GetName().Name)
			+ "\\authentication.db";
		public KeyAuthenticationOptions() { }
	}
}