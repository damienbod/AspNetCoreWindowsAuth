using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppAuthorizationService
{
    public class ValuesCheckRouteParameterHandler : AuthorizationHandler<ValuesRouteRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValuesCheckRouteParameterHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValuesRouteRequirement requirement)
        {
            var routeValues = _httpContextAccessor.HttpContext.Request.RouteValues;
            var routeEndpoint = context.Resource as RouteEndpoint;

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