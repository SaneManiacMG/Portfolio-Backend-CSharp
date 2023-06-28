using Microsoft.AspNetCore.Authorization;

namespace Portfolio.Backend.Csharp.Configs
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; }
        public RoleRequirement(string role) {  Role = role; }
    }
}
