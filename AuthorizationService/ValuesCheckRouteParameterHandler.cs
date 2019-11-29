using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
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
            //var routeEndpoint = context.Resource as RouteEndpoint;
            var routeValues = _httpContextAccessor.HttpContext.Request.RouteValues;
            
            var queryString = _httpContextAccessor.HttpContext.Request.Query;
            var fruit = queryString["fruit"];

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