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

        const string _accountBlocked = "Account is blocked";
        const string _accountUnverified = "Account is not verified";
        const string _incorrectUsernameOrPassword = "Incorrect username or password";
        const string _passwordReset = "Password reset";

        public async Task<string> AuthenticateUser(LoginRequest authenticationRequest)
        {
            User foundUser = await _userService.GetUser(authenticationRequest.UserId);
            if (foundUser == null)
            {
                return _incorrectUsernameOrPassword;
            }

            Login loginDetails = await _loginRepository.GetUserByIdAsync(foundUser.UserId);
            if (loginDetails == null)
            {
                return _incorrectUsernameOrPassword;
            }

            if (loginDetails.AccountStatus == AccountStatus.Unverified)
            {
                return _accountUnverified;
            }

            bool passwordMatch = BCrypt.Net.BCrypt.Verify(authenticationRequest.Password, loginDetails.Password);
            if (!passwordMatch)
            {
                return _incorrectUsernameOrPassword;
            }

            return _jwtAuthenticationManager.Authenticate(foundUser.UserId, foundUser.Role);
        }

        public async Task<string> ResetPassword(LoginRequest loginRequest)
        {
            User foundUser = await _userService.GetUser(loginRequest.UserId);
            if (foundUser == null)
            {
                return _incorrectUsernameOrPassword;
            }

            Login loginDetails = await _loginRepository.GetUserByIdAsync(foundUser.UserId);
            if (loginDetails == null)
            {
                return _incorrectUsernameOrPassword;
            }

            if (loginDetails.AccountStatus == AccountStatus.Blocked)
            {
                return _accountBlocked;
            }

            loginDetails.Password = GenerateSaltAndHash(loginRequest.Password);
            loginDetails.AccountStatus = AccountStatus.Active;

            await _loginRepository.UpdateUserAsync(loginDetails);
            return _passwordReset;
        }

        private string GenerateSaltAndHash(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }
    }
}
