using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppAuthorizationService
{
    public class IsAdminHandler : AuthorizationHandler<IsAdminRequirement>
    {
        private IAppAuthorizationService _appAuthorizationService;

        public IsAdminHandler(IAppAuthorizationService appAuthorizationService)
        {
            _appAuthorizationService = appAuthorizationService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdminRequirement requirement)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (requirement == null)
                throw new ArgumentNullException(nameof(requirement));

            var claimIdentityprovider = context.User.Claims.FirstOrDefault(t => t.Type == "http://schemas.microsoft.com/identity/claims/identityprovider");

            if (claimIdentityprovider != null && _appAuthorizationService.IsAdmin(context.User.Identity.Name, claimIdentityprovider.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}