using AuthenticationApi.Classes;

namespace AuthenticationApi.Interfaces
{
	public interface ICryptionService
	{
		string Hash(string password);
		User DecryptUser(User cipherUser);
		LoginRequest DecryptLoginRequest(LoginRequest cipherRequest);
		LogOutRequest DecryptLogOutRequest(LogOutRequest cipherRequest);
		string Decrypt(string cipherText);
	}
}
