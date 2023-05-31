using Portfolio.Backend.Csharp.Models.Entities;
using Portfolio.Backend.Csharp.Models.Responses;
using Portfolio.Backend.Csharp.Models.User;
using Portfolio.Backend.Csharp.Models.User.Requests;

namespace Portfolio.Backend.Csharp.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponse> AddUser(UserRequest userRequest);
        public Task<UserResponse> UpdateUser(UserRequest userRequest);
        public Task<UserResponse> DeleteUser(string userId);
        public Task<UserResponse> GetUserResponse(UserRequest userRequest);
        public Task<List<UserResponse>> GetAllUsersResponse();
        public Task<User> GetUser(string username, string email);

    }
}
