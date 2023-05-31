using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Backend.Csharp.Configs;
using Portfolio.Backend.Csharp.Interfaces;
using Portfolio.Backend.Csharp.Models.User.Requests;

namespace Portfolio.Backend.Csharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;

        public UserController(IUserService userService, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _userService = userService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpGet]
        [Route("/GetUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsersResponse());
        }

        [HttpPost]
        [Route("/GetUser")]
        public async Task<IActionResult> GetUser([FromBody] UserRequest userRequest)
        {
            var user = await _userService.GetUserResponse(userRequest);
            if (user == null)
            {
                return BadRequest("User Not Found");
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("/AddUser")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] UserRequest userRequest)
        {
            var newUser = await _userService.AddUser(userRequest);
            if (newUser == null)
            {
                return BadRequest("User Already Exists");
            }

            return Ok(newUser);
        }

        [HttpPut]
        [Route("/UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserRequest userRequest)
        {
            var user = await _userService.UpdateUser(userRequest);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpDelete]
        [Route("/DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string userId)
        {
            var deletedUser = await _userService.DeleteUser(userId);
            if (deletedUser == null)
            {
                return BadRequest("User not found");
            }

            return Ok(deletedUser);
        }
    }
}
