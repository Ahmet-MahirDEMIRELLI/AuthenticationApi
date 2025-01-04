using AuthenticationApi.Classes;
using AuthenticationApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserOperationsController : ControllerBase
	{
		private readonly IUserOperationsService _userOperationsService;
		private readonly ICryptionService _cryptionService;
		public UserOperationsController(IUserOperationsService userOperationsService, ICryptionService cryptionService)
		{
			_userOperationsService = userOperationsService;
			_cryptionService = cryptionService;
		}

		[HttpPost("sign-up")]
		public async Task<IActionResult> SignUp([FromBody] User cipherUser)
		{
			if (string.IsNullOrEmpty(cipherUser.UserName) ||
				string.IsNullOrEmpty(cipherUser.Password) ||
				string.IsNullOrEmpty(cipherUser.Email))
			{
				return BadRequest("Empty Field");
			}

			User plainUser = _cryptionService.DecryptUser(cipherUser);
			string answer = await _userOperationsService.DoesExists(plainUser.Email, plainUser.UserName);
			if (answer.Equals("ok"))
			{
				plainUser.Password = _cryptionService.Hash(plainUser.Password);
				var newUser = await _userOperationsService.AddUser(plainUser);
				if (newUser != null)
				{
					return Ok("Your id is: " + newUser.UserId);
				}
				else
				{
					return Problem("server problem");
				}
			}
			else if (answer.Equals("email"))
			{
				return Problem("Email is already in use");
			}

			return Problem("UserName taken");
		}
		
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest cipherRequest)
		{
			if (string.IsNullOrEmpty(cipherRequest.UserName) || string.IsNullOrEmpty(cipherRequest.Password))
			{
				return BadRequest("Empty Fields");
			}

			LoginRequest plainRequest = _cryptionService.DecryptLoginRequest(cipherRequest);
			if (await _userOperationsService.IsAlreadyLoggedIn(plainRequest.UserName))
			{
				return BadRequest("Already Logged In");
			}
			plainRequest.Password = _cryptionService.Hash(plainRequest.Password);
			var user = await _userOperationsService.Login(plainRequest);
			if (user != null)
			{
				string uniqueId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
				LoggedUser loggedUser = new LoggedUser()
				{
					LogId = 0,
					UserId = user.UserId,
					LogInDateTime = DateTime.Now,
					SpecialCode = uniqueId
				};
				LoggedUser newLoggedUser = await _userOperationsService.AddLog(loggedUser);
				if (newLoggedUser != null)
				{
					return Ok("Your special code: " + uniqueId);
				}
				return Problem("Server Problem");
			}
			return BadRequest("Server Problem");
		}

		[HttpDelete("log_out")]
		public async Task<IActionResult> LogOut([FromBody] LogOutRequest cipherRequest)
		{
			if (string.IsNullOrEmpty(cipherRequest.UserName) || string.IsNullOrEmpty(cipherRequest.SpecialCode))
			{
				return BadRequest("Empty Field");
			}

			LogOutRequest plainRequest = _cryptionService.DecryptLogOutRequest(cipherRequest);
			var loggedUser = await _userOperationsService.GetLoggedUser(plainRequest);
			if (loggedUser != null)
			{
				await _userOperationsService.Logout(loggedUser);
				return Ok();
			}
			return BadRequest("User is not logged in");
		}

		[HttpGet("get-user-info/{id:int}&{specialCode}"), ActionName("GetUserInfoAsync")]
		public async Task<IActionResult> GetUserInfoAsync([FromRoute] int id, [FromRoute] string specialCode)
		{
			if (string.IsNullOrEmpty(specialCode))
			{
				return BadRequest("Empty Field");
			}

			var userInfo = await _userOperationsService.GetUserInfo(new GetMyInfoRequest() { UserId = id, SpecialCode = specialCode });
			if (userInfo != null)
			{
				return Ok(userInfo);
			}
			return BadRequest("Invalid Credentials");
		}
	}
}
