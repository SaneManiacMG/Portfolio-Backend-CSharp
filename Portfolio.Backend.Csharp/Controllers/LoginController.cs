using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Backend.Csharp.Configs;
using Portfolio.Backend.Csharp.Interfaces;
using Portfolio.Backend.Csharp.Models.Requests;
using Portfolio.Backend.Csharp.Models.Responses;

namespace Portfolio.Backend.Csharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest loginRequest)
        {
            return ResponseMapping(await _loginService.AuthenticateUser(loginRequest));
        }

        [HttpPost]
        [Route("/ResetPassword")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] LoginRequest loginRequest)
        {
            return ResponseMapping(await _loginService.ResetPassword(loginRequest));
        }

        private IActionResult ResponseMapping(string response)
        {
            const string _accountBlocked = "Account is blocked";
            const string _accountUnverified = "Account is not verified";
            const string _incorrectUsernameOrPassword = "Incorrect username or password";
            const string _passwordReset = "Password reset";

            switch (response)
            {
                case _accountBlocked:
                    return Unauthorized(new MessageResponse(_accountBlocked));
                case _accountUnverified:
                    return Unauthorized(new MessageResponse(_accountUnverified));
                case _incorrectUsernameOrPassword:
                    return Unauthorized(new MessageResponse(_incorrectUsernameOrPassword));
                case _passwordReset:
                    return Ok(new MessageResponse(_passwordReset));
                default:
                    return Ok(new MessageResponse(response));
            }
        }
    }
}
