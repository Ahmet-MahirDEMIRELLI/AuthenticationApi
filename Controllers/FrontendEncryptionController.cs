using AuthenticationApi.Classes;
using AuthenticationApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Controllers
{
	[ApiController]
	public class FrontendEncryptionController : Controller
	{
		private readonly IFrontendEncryption _frontendEncryptionService;
		public FrontendEncryptionController(IFrontendEncryption frontendEncryptionService)
		{
			_frontendEncryptionService = frontendEncryptionService;
		}

		[HttpPost("get-encrypted-user")]
		public IActionResult GetEncryptedUserAsync([FromBody] User plainUser)
		{
			User cipherUser = _frontendEncryptionService.EncryptUser(plainUser);
			if (cipherUser == null || cipherUser.UserName.Equals("") || cipherUser.Password.Equals("") || cipherUser.Email.Equals(""))
			{
				return Problem("Empty Field");
			}
			return Ok(cipherUser);
		}

		[HttpPost("get-encrypted-login-request")]
		public IActionResult GetEncryptedLoginRequestAsync([FromBody] LoginRequest plainRequest)
		{
			LoginRequest cipherRequest = _frontendEncryptionService.EncryptLoginRequest(plainRequest);
			if (cipherRequest == null || cipherRequest.UserName.Equals("") || cipherRequest.Password.Equals(""))
			{
				return Problem("Empty Field");
			}
			return Ok(cipherRequest);
		}

		[HttpPost("get-encrypted-logout-request")]
		public IActionResult GetEncryptedLogOutRequestAsync([FromBody] LogOutRequest plainRequest)
		{
			LogOutRequest cipherRequest = _frontendEncryptionService.EncryptLogOutRequest(plainRequest);
			if (cipherRequest == null || cipherRequest.UserName.Equals("") || cipherRequest.SpecialCode.Equals(""))
			{
				return Problem("Empty Field");
			}
			return Ok(cipherRequest);
		}
	}
}
