using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AppAuthorizationService;

public class ValuesCheckQueryParameterHandler : AuthorizationHandler<ValuesCheckQueryParamRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ValuesCheckQueryParameterHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValuesCheckQueryParamRequirement requirement)
    {
        var queryString = _httpContextAccessor.HttpContext.Request.Query;
        var fruit = queryString["fruit"];

        if (fruit.ToString() == "orange")
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}