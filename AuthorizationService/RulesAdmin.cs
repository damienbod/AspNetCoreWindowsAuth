using System;
using System.Collections.Generic;
using System.Text;

namespace AppAuthorizationService
{
    public static class RulesAdmin
    {

        private static List<string> adminUsers = new List<string>();

        private static List<string> adminProviders = new List<string>();

        public static bool IsAdmin(string username, string providerClaimValue)
        {
            if (adminUsers.Count == 0)
            {
                AddAllowedUsers();
                AddAllowedProviders();
            }

            if (adminUsers.Contains(username) && adminProviders.Contains(providerClaimValue))
            {
                return true;
            }

            return false;
        }

        private static void AddAllowedUsers()
        {
            adminUsers.Add("SWISSANGULAR\\Damien");
        }

        private static void AddAllowedProviders()
        {
            adminProviders.Add("Windows");
        }
    }
}
