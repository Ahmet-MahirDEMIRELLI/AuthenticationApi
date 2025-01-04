using AuthenticationApi.Classes;
using AuthenticationApi.Context;
using AuthenticationApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApi.Repositories
{
	public class UserOperationsRepository : IUserOperationsService
	{
		private readonly AuthAppDbContext _authAppDbContext;
		public UserOperationsRepository(AuthAppDbContext authAppDbContext)
		{
			_authAppDbContext = authAppDbContext;
		}

		public async Task<LoggedUser> AddLog(LoggedUser user)
		{
			var newLoggedUser = await _authAppDbContext.LoggedUsers.AddAsync(user);
			await _authAppDbContext.SaveChangesAsync();
			return newLoggedUser.Entity;
		}

		public async Task<User> AddUser(User user)
		{
			var newUser = await _authAppDbContext.Users.AddAsync(user);
			await _authAppDbContext.SaveChangesAsync();
			return newUser.Entity;
		}

		public async Task<string> DoesExists(string email, string userName)
		{
			bool doesEmailExists = await _authAppDbContext.Users.AnyAsync(x => x.Email == email);
			bool doesUserNameExists = await _authAppDbContext.Users.AnyAsync(x => x.UserName == userName);
			if (!doesEmailExists && !doesUserNameExists)
			{
				return "ok";
			}
			else if (doesEmailExists)
			{
				return "email";
			}
			else
			{
				return "username";
			}
		}

		public async Task<LoggedUser?> GetLoggedUser(LogOutRequest request)
		{
			var user = await _authAppDbContext.Users.FirstOrDefaultAsync(x => x.UserName.Equals(request.UserName));
			if (user == null)
			{
				return null;
			}
			return await _authAppDbContext.LoggedUsers.FirstOrDefaultAsync(x => x.UserId == user.UserId && x.SpecialCode.Equals(request.SpecialCode));
		}

		public async Task<UserInfoAnswer?> GetUserInfo(GetMyInfoRequest request)
		{
			var loggedUser = await _authAppDbContext.LoggedUsers.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.SpecialCode == request.SpecialCode);
			if (loggedUser == null)
			{
				return null;
			}

			var user = await _authAppDbContext.Users.FirstOrDefaultAsync(x => x.UserId == loggedUser.UserId);
			if (user == null)
			{
				return null;
			}

			UserInfoAnswer answer = new UserInfoAnswer() 
			{ 
				Email = user.Email,
				UserName = user.UserName
			};
			return answer;
		}

		public async Task<bool> IsAlreadyLoggedIn(string userName)
		{
			var user = await _authAppDbContext.Users.FirstOrDefaultAsync(x => x.UserName.Equals(userName));
			if (user == null)
			{
				return true;
			}
			return await _authAppDbContext.LoggedUsers.AnyAsync(x => x.UserId == user.UserId);
		}

		public async Task<User?> Login(LoginRequest request)
		{
			return await _authAppDbContext.Users.FirstOrDefaultAsync(x => x.UserName.Equals(request.UserName) && x.Password.Equals(request.Password));
		}

		public async Task Logout(LoggedUser user)
		{
			_authAppDbContext.LoggedUsers.Remove(user);
			await _authAppDbContext.SaveChangesAsync();
		}
	}
}
