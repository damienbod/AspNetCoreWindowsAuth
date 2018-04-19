using System;

namespace AppAuthorizationService
{
    public class AppAuthorizationService : IAppAuthorizationService
    {
        public bool IsAdmin(string username, string providerClaimValue)
        {
            return RulesAdmin.IsAdmin(username, providerClaimValue);
        }
    }
}
