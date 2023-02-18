using AspNetCore.Identity.LiteDB.Data;
using LiteDB;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace JonathanBout.Authentication
{
	internal class KeyAuthenticator : IAuthenticator
	{
		private readonly int keySize;
		private readonly ILiteCollection<KeyUserIdentifier> collection;
		private readonly ILogger<KeyAuthenticator> logger;
		public KeyAuthenticator(ILiteDbContext dbContext, ILogger<KeyAuthenticator> logger)
		{
			keySize = KeyAuthenticationOptions.Instance.KeySize;
			collection = dbContext.LiteDatabase.GetCollection<KeyUserIdentifier>();
			this.logger = logger;
		}

		public (string key, UserIdentifier id) AddUser()
		{
			return AddUser(false);
		}

		private (string key, UserIdentifier id) AddUser(bool suppressLog)
		{
			var nextIdInt = 1;
			if (collection.Count() > 0)
				nextIdInt = collection.Max(x => x.UserId) + 1;

			var id = KeyUserIdentifier.Create(nextIdInt, keySize);
			collection.Insert(id.identifier);
			if (!suppressLog)
				logger.LogInformation("User added with id {id}", id.identifier.UserId);
			return id;
		}

		public UserIdentifier? Authenticate(string hashedKey, int userId = 0)
		{
			if (userId > 0)
			{
				var id = collection.FindById(userId);
				return Hasher.Compare(hashedKey, id.KeyHash) ? id : null;
			}
			foreach (var id in collection.FindAll())
			{
				if (Hasher.Compare(hashedKey, id.KeyHash))
				{
					return id;
				}
			}
			return null;
		}

		public bool DeleteUser(int userId)
		{
			return collection.Delete(userId);
		}

		public UserIdentifier? Login(string key, int userId)
		{
			if (userId > 0)
			{
				var id = collection.FindById(userId);
				if (id is null)
					return null;
				return Hasher.Compare(id.KeyHash, key)? id : null;
			}
			return null;
		}

		public void Initialize()
		{
			if (collection.Count() == 0)
			{
				var (key, id) = AddUser(true);
				logger.LogWarning("Added a default user with key '{key}' and userId '{userId}'" +
					" as the database was empty. Be sure to clear this message when you have" +
					" authenticated to prevent the key from being used by bad guys.", key, id.UserId);
			}
		}
	}
}