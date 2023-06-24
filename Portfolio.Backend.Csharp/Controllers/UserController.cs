using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Backend.Csharp.Configs;
using Portfolio.Backend.Csharp.Interfaces;
using Portfolio.Backend.Csharp.Models.Requests;

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
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsersResponse());
        }

        [HttpPost]
        [Route("/GetUser")]
        // TODO: Add Authorize attribute
        // TODO: Change from HttpPost to HttpGet
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
        // TODO: Add Authorize attribute
        public async Task<IActionResult> UpdateUser([FromBody] UserRequest userRequest)
        {
            var user = await _userService.UpdateUser(userRequest, null);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpDelete]
        [Route("/DeleteUser/{userId}")]
        // TODO: Add Authorize attribute
        public async Task<IActionResult> DeleteUser([FromRoute] string userId)
        {
            var deletedUser = await _userService.DeleteUser(userId);
            if (deletedUser == null)
            {
                return BadRequest("User not found");
            }

            return Ok(deletedUser);
        }

        [HttpPost]
        [Route("/UpdateRole/{userId}/Role/{role}")]
        // TODO: Add Authorize attribute
        public async Task<IActionResult> UpdateAccountType(string userId, string role) { 

            var user = await _userService.UpdateUserRole(userId, role);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            return Ok(user);
        }
    }
}
