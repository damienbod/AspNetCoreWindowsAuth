using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace AppAuthorizationService;

public class ValuesCheckRequestBodyHandler : AuthorizationHandler<ValuesRequestBodyRequirement, BodyData>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValuesRequestBodyRequirement requirement, BodyData bodyData)
    {
        if (bodyData.User == "mike")
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class BodyData
{
    public string User { get; set; }
}