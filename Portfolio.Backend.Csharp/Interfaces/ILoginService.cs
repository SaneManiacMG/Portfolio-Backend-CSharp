using Portfolio.Backend.Csharp.Models.Requests;

namespace Portfolio.Backend.Csharp.Interfaces
{
    public interface ILoginService
    {
        Task<string> AuthenticateUser(LoginRequest loginRequest);
        Task<string> RegisterUser(LoginRequest logRequest);
        Task<string> UpdatePassword(LoginRequest loginRequest);
    }
}
