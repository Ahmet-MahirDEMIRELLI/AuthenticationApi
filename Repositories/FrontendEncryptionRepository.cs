using AuthenticationApi.Classes;
using AuthenticationApi.Interfaces;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System.Text;

namespace AuthenticationApi.Repositories
{
	public class FrontendEncryptionRepository : IFrontendEncryption
	{
		private readonly string _backendPublicKeyPath = "Resources/Keys/backendPublic.pem";
        public FrontendEncryptionRepository(){}

		public LoginRequest EncryptLoginRequest(LoginRequest plainRequest)
		{
			LoginRequest cipherRequest = new LoginRequest()
			{
				UserName = Encrypt(plainRequest.UserName),
				Password = Encrypt(plainRequest.Password)
			};
			return cipherRequest;
		}

		public LogOutRequest EncryptLogOutRequest(LogOutRequest plainRequest)
		{
			LogOutRequest cipherRequest = new LogOutRequest()
			{
				UserName = Encrypt(plainRequest.UserName),
				SpecialCode = Encrypt(plainRequest.SpecialCode),
			};
			return cipherRequest;
		}

		public User EncryptUser(User plainUser)
		{
			User cipherUser = new User()
			{
				UserId = plainUser.UserId,
				UserName = Encrypt(plainUser.UserName),
				Password = Encrypt(plainUser.Password),
				Email = Encrypt(plainUser.Email)
			};
			return cipherUser;
		}

		public string Encrypt(string plainText)
		{
			try
			{
				if (!File.Exists(_backendPublicKeyPath))
				{
					return "key not found";
				}

				var publicKeyPem = File.ReadAllText(_backendPublicKeyPath);
				var publicKey = GetPublicKeyFromPem(publicKeyPem);

				var rsaEngine = new Pkcs1Encoding(new RsaEngine());
				rsaEngine.Init(true, publicKey);

				var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
				var encryptedBytes = rsaEngine.ProcessBlock(plainTextBytes, 0, plainTextBytes.Length);

				// Şifrelenmiş veriyi URL dostu hale getir
				var base64EncryptedData = Convert.ToBase64String(encryptedBytes);
				var urlSafeEncryptedData = base64EncryptedData
					.Replace("+", "-")
					.Replace("/", "_")
					.TrimEnd('=');

				return urlSafeEncryptedData;
			}
			catch (Exception)
			{
				return "exception";
			}
		}

		private static RsaKeyParameters GetPublicKeyFromPem(string pem)
		{
			using (var reader = new StringReader(pem))
			{
				var pemReader = new PemReader(reader);
				return (RsaKeyParameters)pemReader.ReadObject();
			}
		}
	}
}
