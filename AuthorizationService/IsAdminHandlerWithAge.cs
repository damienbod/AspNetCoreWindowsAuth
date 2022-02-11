using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppAuthorizationService;

public class IsAdminHandlerWithAge
    : AuthorizationHandler<IsAdminRequirement, AdminData>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IsAdminRequirement requirement, AdminData adminData)
    {
        // Do null checks
        var claim = context.User.Claims.FirstOrDefault(t => t.Type == "claim name");
        if (claim != null && Validate(context.User.Identity.Name, claim.Value, adminData))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

    private bool Validate(string name, string value, AdminData adminData)
    {
        throw new NotImplementedException();
    }

    private IAppAuthorizationService _appAuthorizationService;

    public IsAdminHandlerWithAge(IAppAuthorizationService appAuthorizationService)
    {
        _appAuthorizationService = appAuthorizationService;
    }
}