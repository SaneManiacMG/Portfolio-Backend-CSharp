using Portfolio.Backend.Csharp.Models.Entities;
using Portfolio.Backend.Csharp.Models.Enums;
using Portfolio.Backend.Csharp.Models.Requests;
using Portfolio.Backend.Csharp.Models.Responses;

namespace Portfolio.Backend.Csharp.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponse> AddUser(UserRequest userRequest);
        public Task<UserResponse> UpdateUser(UserRequest userRequest, Role? role);
        public Task<UserResponse> DeleteUser(string userId);
        public Task<UserResponse> GetUserResponse(UserRequest userRequest);
        public Task<List<UserResponse>> GetAllUsersResponse();
        public Task<User> GetUser(string username, string email);
        public Task<UserResponse> UpdateUserRole(string userId, string userRole);

    }
}
