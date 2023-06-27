using Microsoft.AspNetCore.Authentication.JwtBearer;
using Portfolio.Backend.Csharp.Models.Enums;
using System.Security.Claims;

namespace Portfolio.Backend.Csharp.Configs
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            return base.AuthenticationFailed(context);
        }

        public override Task Challenge(JwtBearerChallengeContext context)
        {
            return base.Challenge(context);
        }

        public override Task Forbidden(ForbiddenContext context)
        {
            return base.Forbidden(context);
        }

        public override Task MessageReceived(MessageReceivedContext context)
        {
            return base.MessageReceived(context);
        }

        public override Task TokenValidated(TokenValidatedContext context)
        {
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity?.FindFirst(ClaimTypes.UserData);
            var roleClaim = claimsIdentity?.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || roleClaim == null)
            {
                context.Fail("Invalid token");
                return base.TokenValidated(context);
            }

            var userId = userIdClaim.Value;
            var role = Enum.Parse<Role>(roleClaim.Value);

            context.Request.HttpContext.Items["UserId"] = userId;
            context.Request.HttpContext.Items["Role"] = role;

            return Task.CompletedTask;
        }
    }
}
