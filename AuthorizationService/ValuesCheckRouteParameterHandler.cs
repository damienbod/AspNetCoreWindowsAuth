using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

            object user;
            routeValues.TryGetValue("user", out user);
            if (user.ToString() == "phil")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}