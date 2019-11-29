using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppAuthorizationService
{
    public class ValuesCheckRouteParameterHandler : AuthorizationHandler<ValuesRouteRequirement>
    {
        private IAppAuthorizationService _appAuthorizationService;

        public ValuesCheckRouteParameterHandler(IAppAuthorizationService appAuthorizationService)
        {
            _appAuthorizationService = appAuthorizationService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValuesRouteRequirement requirement)
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