using System.Reflection;

namespace JonathanBout.Authentication
{
	public abstract class AuthenticationOptions<TSelf>
		where TSelf : AuthenticationOptions<TSelf>, new()
	{
		internal static TSelf Instance { get; set; } = new();

		/// <summary>
		/// The name of the cookie. Default is <c>"AuthCookie"</c>.
		/// </summary>
		public virtual string CookieName { get; set; } = "AuthCookie";

		/// <summary>
		/// The base path for the endpoints. Here, the /Key and /Login endpoints will be created. Default is <c>"/Authentication"</c>.
		/// </summary>
		public virtual string BasePath { get; set; } = "/Authentication";

		/// <summary>
		/// The url to the database file. Default is <c>"C:\Users\Public\Documents\[Assembly Name]\authentication.db"</c>.
		/// </summary>
		public string DatabasePath { get; set; } = "C:\\Users\\Public\\Documents\\"
			+ (Assembly.GetEntryAssembly()?.GetName().Name ?? Assembly.GetExecutingAssembly().GetName().Name)
			+ "\\authentication.db";

	}
}