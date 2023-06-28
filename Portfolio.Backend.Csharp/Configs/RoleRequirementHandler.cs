using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Portfolio.Backend.Csharp.Configs
{
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == requirement.Role))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
