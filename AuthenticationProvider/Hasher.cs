using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace JonathanBout.Authentication
{
	internal static class Hasher
	{
		public static int HashSize => KeyAuthenticationOptions.Instance.KeySize;
		private const int SaltSize = 64;
		public static string Hash(string password)
		{
			byte[] saltBuffer;
			byte[] hashBuffer;
			using (var keyDerivation = new Rfc2898DeriveBytes(password, SaltSize, 10000, HashAlgorithmName.SHA512))
			{
				saltBuffer = keyDerivation.Salt;
				hashBuffer = keyDerivation.GetBytes(HashSize);
			}

			byte[] result = new byte[HashSize + SaltSize];
			Buffer.BlockCopy(hashBuffer, 0, result, 0, HashSize);
			Buffer.BlockCopy(saltBuffer, 0, result, HashSize, SaltSize);
			return Convert.ToBase64String(result);
		}

		public static bool Compare(string hash, string value)
		{
			byte[] hashedPasswordBytes = Convert.FromBase64String(hash);

			if (hashedPasswordBytes.Length != HashSize + SaltSize)
			{
				return false;
			}

			byte[] hashBytes = new byte[HashSize];
			Buffer.BlockCopy(hashedPasswordBytes, 0, hashBytes, 0, HashSize);
			byte[] saltBytes = new byte[SaltSize];
			Buffer.BlockCopy(hashedPasswordBytes, HashSize, saltBytes, 0, SaltSize);

			byte[] providedHashBytes;
			using (var keyDerivation = new Rfc2898DeriveBytes(value, saltBytes, 10000, HashAlgorithmName.SHA512))
			{
				providedHashBytes = keyDerivation.GetBytes(HashSize);
			}

			return CompareByteArrays(hashBytes, providedHashBytes);
		}

		static bool CompareByteArrays(byte[] x, byte[] y)
		{
			if (x == y)
			{
				return true;
			}

			if (x == null || y == null || x.Length != y.Length)
			{
				return false;
			}

			for (int i = 0; i < x.Length; i++)
			{
				if (x[i] != y[i])
					return false;
			}

			return true;
		}

	}
}
