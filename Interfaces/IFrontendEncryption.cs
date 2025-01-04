using AuthenticationApi.Classes;

namespace AuthenticationApi.Interfaces
{
	public interface IFrontendEncryption
	{
		User EncryptUser(User plainUser);
		LoginRequest EncryptLoginRequest(LoginRequest plainRequest);
		LogOutRequest EncryptLogOutRequest(LogOutRequest plainRequest);
		string Encrypt(string plainText);
	}
}
