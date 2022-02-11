using Microsoft.AspNetCore.Authorization;

namespace AppAuthorizationService;

public static class MyPolicies
{
    private static AuthorizationPolicy requireWindowsProviderPolicy;

    public static AuthorizationPolicy GetRequireWindowsProviderPolicy()
    {
        if (requireWindowsProviderPolicy != null) return requireWindowsProviderPolicy;

        requireWindowsProviderPolicy = new AuthorizationPolicyBuilder()
                .RequireClaim("http://schemas.microsoft.com/identity/claims/identityprovider", "Windows")
                .Build();

        return requireWindowsProviderPolicy;
    }
}
