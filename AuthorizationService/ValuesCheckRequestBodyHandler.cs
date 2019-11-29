using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppAuthorizationService
{
    public class ValuesCheckRequestBodyHandler : AuthorizationHandler<ValuesRequestBodyRequirement>
    {
        private IAppAuthorizationService _appAuthorizationService;

        public ValuesCheckRequestBodyHandler(IAppAuthorizationService appAuthorizationService)
        {
            _appAuthorizationService = appAuthorizationService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValuesRequestBodyRequirement requirement)
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