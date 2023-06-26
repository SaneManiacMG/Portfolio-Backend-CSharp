using Portfolio.Backend.Csharp.Models.Entities;
using Portfolio.Backend.Csharp.Models.Enums;
using Portfolio.Backend.Csharp.Models.Requests;
using Portfolio.Backend.Csharp.Models.Responses;

namespace Portfolio.Backend.Csharp.Interfaces
{
    public interface IUserService
    {
        public Task<object> AddUser(UserRequest userRequest);
        public Task<object> UpdateUser(UserRequest userRequest, Role? role);
        public Task<object> DeleteUser(string userId);
        public Task<object> GetUserResponse(string userId);
        public Task<List<UserResponse>> GetAllUsersResponse();
        public Task<User> GetUser(string userId);
        public Task<object> UpdateUserRole(string userId, string userRole);

    }
}
