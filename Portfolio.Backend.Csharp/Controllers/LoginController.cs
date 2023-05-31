using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Backend.Csharp.Configs;
using Portfolio.Backend.Csharp.Interfaces;
using Portfolio.Backend.Csharp.Models.Requests;

namespace Portfolio.Backend.Csharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;

        public LoginController(ILoginService loginService, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _loginService = loginService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest loginRequest)
        {
            string response = await _loginService.AuthenticateUser(loginRequest);
            if (response == null)
            {
                return NotFound("Invalid Username/Email or password");
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("/Register")]
        public async Task<IActionResult> RegisterUser([FromBody] LoginRequest loginRequest)
        {
            string response = await _loginService.RegisterUser(loginRequest);
            if (response == null)
            {
                return NotFound("Invalid Username/Email or Password");
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("/ResetPassword")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] LoginRequest loginRequest)
        {
            string response = await _loginService.UpdatePassword(loginRequest);
            if (response == null)
            {
                return NotFound("Invalid Username/Email or Password");
            }

            return Ok(response);
        }
    }
}
