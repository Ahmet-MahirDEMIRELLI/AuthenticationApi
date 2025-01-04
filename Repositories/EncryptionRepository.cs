using AuthenticationApi.Classes;
using AuthenticationApi.Interfaces;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationApi.Repositories
{
	public class EncryptionRepository : IEncryptionService
	{
		private readonly string _backendPrivateKeyPath = "Resources/Keys/backendPrivate.pem";
		public EncryptionRepository() {}
		public string Decrypt(string cipherText)
		{
			try
			{
				if (!File.Exists(_backendPrivateKeyPath))
				{
					return "key not found";
				}

				var privateKeyPem = File.ReadAllText(_backendPrivateKeyPath);
				var privateKey = GetPrivateKeyFromPem(privateKeyPem);
				var rsaEngine = new Pkcs1Encoding(new RsaEngine());

				rsaEngine.Init(false, privateKey);
				var encryptedData = Convert.FromBase64String(cipherText);

				var decryptedBytes = rsaEngine.ProcessBlock(encryptedData, 0, encryptedData.Length);
				return Encoding.UTF8.GetString(decryptedBytes);
			}
			catch (Exception)
			{
				return "exception";
			}
		}

		public User DecryptUser(User cipherUser)
		{
			User plainUser = new User()
			{
				UserId = cipherUser.UserId,
				UserName = Decrypt(cipherUser.UserName),
				Password = Decrypt(cipherUser.Password),
				Email = Decrypt(cipherUser.Email)
			};
			return plainUser;
		}

		public LoginRequest DecryptLoginRequest(LoginRequest cipherRequest)
		{
			LoginRequest plainRequest = new LoginRequest()
			{
				UserName = Decrypt(cipherRequest.UserName),
				Password = Decrypt(cipherRequest.Password),
			};
			return plainRequest;
		}

		public LogOutRequest DecryptLogOutRequest(LogOutRequest cipherRequest)
		{
			LogOutRequest plainRequest = new LogOutRequest()
			{
				UserName = Decrypt(cipherRequest.UserName),
				SpecialCode = Decrypt(cipherRequest.SpecialCode),
			};
			return plainRequest;
		}

		public string Hash(string password)
		{
			using (var sha256 = SHA256.Create())
			{
				var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
				return Convert.ToBase64String(hashedBytes);
			}
		}
		private static RsaKeyParameters GetPrivateKeyFromPem(string pem)
		{
			using (var reader = new StringReader(pem))
			{
				var pemReader = new PemReader(reader);
				var keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
				return (RsaKeyParameters)keyPair.Private;
			}
		}
	}
}
