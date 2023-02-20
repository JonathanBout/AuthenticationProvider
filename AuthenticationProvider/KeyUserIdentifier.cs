using LiteDB;
using System.IO.Enumeration;
using System.Security.Cryptography;

namespace JonathanBout.Authentication
{
	internal class KeyUserIdentifier : IUserIdentifier<KeyUserIdentifier>
	{
		public string KeyHash { get; set; } = "";
		public int UserId { get; set; }

		public KeyUserIdentifier() { }

		KeyUserIdentifier(string key)
		{
			KeyHash = Hasher.Hash(key);
		}

		public static (string key, KeyUserIdentifier identifier) Create(int userId)
		{
			var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(KeyAuthenticationOptions.Instance.KeySize));
			return (key, new KeyUserIdentifier(key)
			{
				UserId = userId
			});
		}
	}
}