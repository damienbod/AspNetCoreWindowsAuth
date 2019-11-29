using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppAuthorizationService
{
    public class ValuesCheckRequestBodyHandler : AuthorizationHandler<ValuesRequestBodyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValuesRequestBodyRequirement requirement)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (requirement == null)
                throw new ArgumentNullException(nameof(requirement));

            var claimIdentityprovider = context.User.Claims.FirstOrDefault(t => t.Type == "http://schemas.microsoft.com/identity/claims/identityprovider");

            if (claimIdentityprovider != null)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}