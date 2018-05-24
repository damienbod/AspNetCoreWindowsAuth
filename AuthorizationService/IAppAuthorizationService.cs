using System;

namespace AppAuthorizationService
{
    public interface IAppAuthorizationService
    {
        bool IsAdmin(string username, string providerClaimValue);
        bool BobIsAnAdmin(string name);
    }
}
