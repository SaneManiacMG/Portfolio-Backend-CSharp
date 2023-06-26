using AutoMapper;
using Portfolio.Backend.Csharp.Interfaces;
using Portfolio.Backend.Csharp.Models.Entities;
using Portfolio.Backend.Csharp.Models.Enums;
using Portfolio.Backend.Csharp.Models.Requests;
using Portfolio.Backend.Csharp.Models.Responses;

namespace Portfolio.Backend.Csharp.Services
{
#nullable disable
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISequenceGenerator _sequenceGenerator;
        private readonly IMapper _mapper;
        private readonly ILoginRepository _loginRepository;

        const string _userNotFound = "User not found";
        const string _userAlreadyExists = "User already exists";
        const string _userDeleted = "User deleted";

        public UserService(IUserRepository userRepository, ISequenceGenerator sequenceGenerator, IMapper mapper,
            ILoginRepository loginRepository)
        {
            _userRepository = userRepository;
            _sequenceGenerator = sequenceGenerator;
            _mapper = mapper;
            _loginRepository = loginRepository;
        }

        public async Task<object> AddUser(UserRequest userRequest)
        {
            var userExists = await GetUser(userRequest.Username)!;
            if (userExists != null)
            {
                return "User already exists!";
                // return _mapper.Map<UserResponse>(userExists);
            }

            string generatedUserId = _sequenceGenerator.UserIdSequenceGenerator();
            User newUser = new User(generatedUserId, userRequest, DateTime.Now.ToUniversalTime());

            Login authentication = new Login(generatedUserId, generatedUserId, DateTime.Now.ToUniversalTime());
            await _loginRepository.CreateNewUserAsync(authentication);

            return _mapper.Map<UserResponse>(await _userRepository.AddUserAsync(newUser));
        }

        public async Task<object> DeleteUser(string userId)
        {
            var userExists = await GetUserById(userId);
            if (userExists != null)
            {
                return _mapper.Map<UserResponse>(await _userRepository.DeleteUserAsync(userExists));
            }
            return _userNotFound;
        }

        public async Task<User> GetUser(string userId)
        {
            var usernameExistsByEmail = await GetUserByEmail(userId);
            if (usernameExistsByEmail != null)
            {
                return usernameExistsByEmail;
            }

            var userExistsByUsername = await GetUserByUsername(userId);
            if (userExistsByUsername != null)
            {
                return userExistsByUsername;
            }
            return null;
        }

        private async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        private async Task<User> GetUserById(string userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        private async Task<User> GetUserByUsername(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }

        public async Task<List<User>> GetUsers()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<object> UpdateUser(UserRequest updatedUser, Role? role)
        {
            User updatedDetails = new User();

            var userExistsByUsername = await GetUser(updatedUser.Username);
            if (userExistsByUsername != null)
            {
                updatedDetails = updateObject(userExistsByUsername, role);
            }

            var userExistsByEmail = await GetUser(updatedUser.Email);
            if (userExistsByEmail != null)
            {
                updatedDetails = updateObject(userExistsByEmail, role);
            }

            if (userExistsByUsername == null || userExistsByEmail == null)
            {
                return _userNotFound;
            }

            User userExists = updatedDetails;

            return _mapper.Map<UserResponse>(await _userRepository.UpdateUserAsync(userExists));
        }

        private User updateObject(User update, Role? role)
        {
            User newModel = new User();
            newModel.Username = update.Username;
            newModel.FirstName = update.FirstName;
            newModel.LastName = update.LastName;
            newModel.Email = update.Email;
            newModel.PhoneNr = update.PhoneNr;
            newModel.DateModified = DateTime.Now.ToUniversalTime();

            if (role != null)
            {
                newModel.Role = (Role)role;
            }

            return newModel;
        }

        public async Task<object> GetUserResponse(string userId)
        {
            object response = await GetUser(userId);

            if (response is null)
            {
                return _userNotFound;
            }

            return _mapper.Map<UserResponse>(response);
        }

        public async Task<List<UserResponse>> GetAllUsersResponse()
        {

            List<User> ListOfUsers = await GetUsers();
            if (!ListOfUsers.Any())
            {
                return null;
            }

            List<UserResponse> MappedUserList = new List<UserResponse>();

            foreach (User user in ListOfUsers)
            {
                MappedUserList.Add(_mapper.Map<UserResponse>(user));
            }

            return MappedUserList;
        }

        public async Task<object> UpdateUserRole(string userId, string userRole)
        {
            var userExists = await GetUserById(userId);

            if (userExists == null)
            {
                return _userNotFound;
            }

            switch (userRole)
            {
                case "Admin":
                    userExists.Role = Role.Admin;
                    break;
                case "Owner":
                    userExists.Role = Role.Owner;
                    break;
                default:
                    userExists.Role = Role.Visitor;
                    break;
            }

            return await UpdateUser(_mapper.Map<UserRequest>(userExists), userExists.Role);
        }
    }
}
