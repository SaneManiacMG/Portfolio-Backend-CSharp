using Portfolio.Backend.Csharp.Models.Requests;
using Portfolio.Backend.Csharp.Models.Responses;

namespace Portfolio.Backend.Csharp.Interfaces
{
    public interface ILoginService
    {
        Task<string> AuthenticateUser(LoginRequest loginRequest);
        Task<string> ResetPassword(LoginRequest logRequest);
    }
}
