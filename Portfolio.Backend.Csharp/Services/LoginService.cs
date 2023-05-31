using Microsoft.IdentityModel.Tokens;
using Portfolio.Backend.Csharp.Configs;
using Portfolio.Backend.Csharp.Interfaces;
using Portfolio.Backend.Csharp.Models.Entities;
using Portfolio.Backend.Csharp.Models.Enums;
using Portfolio.Backend.Csharp.Models.Requests;

namespace Portfolio.Backend.Csharp.Services
{
#nullable disable
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IUserService _userService;
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;

        public LoginService(ILoginRepository loginRepository, IUserService userService, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _loginRepository = loginRepository;
            _userService = userService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        public async Task<string> AuthenticateUser(LoginRequest authenticationRequest)
        {
            User foundUser = await _userService.GetUser(authenticationRequest.UserId, authenticationRequest.UserId);
            Login loginDetails = await _loginRepository.GetUserByIdAsync(foundUser.UserId);

            bool userFound = DoesUserExist(foundUser, loginDetails);

            if (!userFound)
            {
                return null;
            }

            bool passwordMatch = BCrypt.Net.BCrypt.Verify(authenticationRequest.Password, loginDetails.Password);

            if (!passwordMatch)
            {
                return null;
            }

            return _jwtAuthenticationManager.Authenticate(foundUser.UserId);
        }

        public async Task<string> RegisterUser(LoginRequest authenticationRequest)
        {
            User foundUser = await _userService.GetUser(authenticationRequest.UserId, authenticationRequest.UserId);
            Login loginDetails = await _loginRepository.GetUserByIdAsync(foundUser.UserId);

            bool userFound = DoesUserExist(foundUser, loginDetails);

            if (!userFound)
            {
                return null;
            }

            loginDetails.Password = GenerateSaltAndHash(authenticationRequest.Password);
            loginDetails.AccountStatus = AccountStatus.Active;

            await _loginRepository.UpdateUserAsync(loginDetails);
            return "User register for login account";
        }

        private string GenerateSaltAndHash(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        private bool DoesUserExist(User user, Login authentication)
        {
            if (user == null)
            {
                return false;
            }

            if (authentication == null)
            {
                return false;
            }

            return true;
        }

        public async Task<string> UpdatePassword(LoginRequest loginRequest)
        {
            User foundUser = await _userService.GetUser(loginRequest.UserId, loginRequest.UserId);
            Login loginDetails = await _loginRepository.GetUserByIdAsync(foundUser.UserId);

            bool userFound = DoesUserExist(foundUser, loginDetails);

            if (!userFound)
            {
                return null;
            }

            loginDetails.Password = GenerateSaltAndHash(loginRequest.Password);
            await _loginRepository.UpdateUserAsync(loginDetails);
            return "Password Updated";
        }
    }
}
