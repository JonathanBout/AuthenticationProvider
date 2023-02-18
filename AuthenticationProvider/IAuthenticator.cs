namespace JonathanBout.Authentication
{
	internal interface IAuthenticator
	{
		/// <summary>
		/// Logs in with a user key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		internal UserIdentifier? Login(string key, int userId);
		/// <summary>
		/// Logs in with a user keyhash, from the cookie
		/// </summary>
		/// <param name="hashedKey"></param>
		/// <returns></returns>
		internal UserIdentifier? Authenticate(string hashedKey, int userId = 0);
		internal (string key, UserIdentifier? id) AddUser();
		internal bool DeleteUser(int userId);
		internal void Initialize();
	}
}