using Portfolio.Backend.Csharp.Models.Entities;

namespace Portfolio.Backend.Csharp.Interfaces
{
    public interface ILoginRepository
    {
        Task<Login> GetUserByIdAsync(string userId);
        Task<Login> CreateNewUserAsync(Login authentication);
        Task<Login> UpdateUserAsync(Login authentication);
        Task<Login> DeleteUserAsync(Login authentication);
    }
}
