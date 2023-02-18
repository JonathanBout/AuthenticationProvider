using LiteDB;
using System.IO.Enumeration;
using System.Security.Cryptography;

namespace JonathanBout.Authentication
{
	internal class KeyUserIdentifier : UserIdentifier
	{
		public string KeyHash { get; set; } = "";

		public KeyUserIdentifier() { }

		KeyUserIdentifier(string key)
		{
			KeyHash = Hasher.Hash(key);
		}

		internal static (string key, KeyUserIdentifier identifier) Create(int userId, int keySize)
		{
			var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(keySize));
			return (key, new KeyUserIdentifier(key)
			{
				UserId = userId
			});
		}

		public override bool CompareString(string hash)
		{
			return Hasher.Compare(hash, KeyHash);
		}
	}
}