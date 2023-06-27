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
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("/GetUsers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsersResponse());
        }

        [HttpGet]
        [Route("/GetUser/{userId}")]
        // TODO: Add Authorize attribute
        // TODO: Change from HttpPost to HttpGet
        public async Task<IActionResult> GetUser([FromRoute] string userId)
        {
            return ResponseMapping(await _userService.GetUserResponse(userId));
        }

        [HttpPost]
        [Route("/AddUser")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] UserRequest userRequest)
        {
            return ResponseMapping(await _userService.AddUser(userRequest));
        }

        [HttpPut]
        [Route("/UpdateUser")]
        // TODO: Add Authorize attribute
        public async Task<IActionResult> UpdateUser([FromBody] UserRequest userRequest)
        {
            return ResponseMapping(await _userService.UpdateUser(userRequest, null));
        }

        [HttpDelete]
        [Route("/DeleteUser/{userId}")]
        // TODO: Add Authorize attribute
        public async Task<IActionResult> DeleteUser([FromRoute] string userId)
        {
            return ResponseMapping(await _userService.DeleteUser(userId));
        }

        [HttpPost]
        [Route("/UpdateRole/{userId}/Role/{role}")]
        // TODO: Add Authorize attribute
        public async Task<IActionResult> UpdateAccountType(string userId, string role)
        {
            return ResponseMapping(await _userService.UpdateUserRole(userId, role));
        }

        private IActionResult ResponseMapping(object response)
        {

            if (response is UserResponse)
            {
                return Ok(response);
            }

            const string _userNotFound = "User not found";
            const string _userAlreadyExists = "User already exists";
            const string _userDeleted = "User deleted";
            const string _internalServerError = "Internal server error";

            switch (response)
            {
                case _userNotFound:
                    return NotFound(new MessageResponse(_userNotFound));
                case _userAlreadyExists:
                    return BadRequest(new MessageResponse(_userAlreadyExists));
                case _userDeleted:
                    return Ok(new MessageResponse(_userDeleted));
                default:
                    // return internal server error
                    return StatusCode(500, new MessageResponse(_internalServerError));
            }
        }
    }
}
