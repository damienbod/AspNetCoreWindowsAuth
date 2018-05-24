using System;

namespace AppAuthorizationService
{
    public class AppAuthorizationService : IAppAuthorizationService
    {
        public bool BobIsAnAdmin(string name)
        {
            if(name.ToLower().Contains("bob"))
            {
                return true;
            }

            return false;
        }

        public bool IsAdmin(string username, string providerClaimValue)
        {
            return RulesAdmin.IsAdmin(username, providerClaimValue);
        }
    }
}
