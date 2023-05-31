using AutoMapper;
using Portfolio.Backend.Csharp.Interfaces;
using Portfolio.Backend.Csharp.Models.Entities;
using Portfolio.Backend.Csharp.Models.Enums;
using Portfolio.Backend.Csharp.Models.Responses;
using Portfolio.Backend.Csharp.Models.User.Requests;

namespace Portfolio.Backend.Csharp.Services
{
#nullable disable
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISequenceGenerator _sequenceGenerator;
        private readonly IMapper _mapper;
        private readonly ILoginRepository _loginRepository;

        public UserService(IUserRepository userRepository, ISequenceGenerator sequenceGenerator, IMapper mapper,
            ILoginRepository loginRepository)
        {
            _userRepository = userRepository;
            _sequenceGenerator = sequenceGenerator;
            _mapper = mapper;
            _loginRepository = loginRepository;
        }

        public async Task<UserResponse> AddUser(UserRequest userRequest)
        {
            var userExists = await GetUser(userRequest.Username, userRequest.Email)!;
            if(userExists != null)
            {
                return _mapper.Map<UserResponse>(userExists);
            }

            string generatedUserId = _sequenceGenerator.UserIdSequenceGenerator();
            User newUser = new User(generatedUserId, userRequest, DateTime.Now);

            Login authentication = new Login(generatedUserId, generatedUserId, DateTime.Now);
            await _loginRepository.CreateNewUserAsync(authentication);

            return _mapper.Map<UserResponse>(await _userRepository.AddUserAsync(newUser));
        }

        public async Task<UserResponse> DeleteUser(string userId)
        {
            var userExists = await GetUserById(userId);
            if(userExists != null)
            {
                return _mapper.Map<UserResponse>(await _userRepository.DeleteUserAsync(userExists));
            }
            return null;
        }

        public async Task<User> GetUser(string username, string email)
        {
            var usernameExistsByEmail = await GetUserByEmail(email);
            if (usernameExistsByEmail != null)
            {
                return usernameExistsByEmail;
            }

            var userExistsByUsername = await GetUserByUsername(username);
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

        public async Task<UserResponse> UpdateUser(UserRequest userRequest)
        {
            User userExists = await GetUser(userRequest.Username, userRequest.Email);

            if (userExists != null)
            {
                userExists.Username = userRequest.Username;
                userExists.FirstName = userRequest.FirstName;
                userExists.LastName = userRequest.LastName;
                userExists.Email = userRequest.Email;
                userExists.PhoneNr = userRequest.PhoneNr;

                return _mapper.Map<UserResponse>(await _userRepository.UpdateUserAsync(userExists));
            }
            return null;
        }

        public async Task<UserResponse> GetUserResponse(UserRequest userRequest)
        {
            return _mapper.Map<UserResponse>(await GetUser(userRequest.Username, userRequest.Email));
        }

        public async Task<List<UserResponse>> GetAllUsersResponse()
        {

            List<User> ListOfUsers = await GetUsers();
            if (!ListOfUsers.Any())
            {
                return null;
            }

            List<UserResponse> MappedUserList = new List<UserResponse>();

            foreach(User user in ListOfUsers)
            {
                MappedUserList.Add(_mapper.Map<UserResponse>(user));
            }

            return MappedUserList;
        }
    }
}
