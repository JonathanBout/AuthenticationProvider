namespace JonathanBout.Authentication
{
	internal interface IAuthenticator
	{
		/// <summary>
		/// Logs in with a user key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		internal IUserIdentifier? Login(string key, int userId);
		/// <summary>
		/// Logs in with a user keyhash, from the cookie
		/// </summary>
		/// <param name="hashedKey"></param>
		/// <returns></returns>
		internal IUserIdentifier? Authenticate(string hashedKey, int userId = 0);
		internal (string key, IUserIdentifier? id) AddUser();
		internal bool DeleteUser(int userId);
		internal void Initialize();
	}
}