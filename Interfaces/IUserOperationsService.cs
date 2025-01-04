using AuthenticationApi.Classes;

namespace AuthenticationApi.Interfaces
{
	public interface IUserOperationsService
	{
		Task<string> DoesExists(string email, string userName);
		Task<bool> IsAlreadyLoggedIn(string userName);
		Task<User> AddUser(User user);
		Task<LoggedUser> AddLog(LoggedUser user);
		Task<User?> Login(LoginRequest request);
		Task Logout(LoggedUser user);
		Task<LoggedUser?> GetLoggedUser(LogOutRequest request);
		Task<UserInfoAnswer?> GetUserInfo(GetMyInfoRequest request);
	}
}
