using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppAuthorizationService
{
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
}
