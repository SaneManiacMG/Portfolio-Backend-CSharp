using Portfolio.Backend.Csharp.Models.Requests;
using Portfolio.Backend.Csharp.Models.Responses;

namespace Portfolio.Backend.Csharp.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResponse> AuthenticateUser(LoginRequest loginRequest);
        Task<string> RegisterUser(LoginRequest logRequest);
        Task<string> UpdatePassword(LoginRequest loginRequest);
    }
}
