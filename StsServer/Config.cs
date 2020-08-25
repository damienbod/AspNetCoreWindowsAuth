// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace StsServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("scope_used_for_hybrid_flow", "Scope for the scope_used_for_hybrid_flow"),
                new ApiScope("native_api",  "Scope for the native_api")
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("ApiHybridFlow")
                {
                    ApiSecrets =
                    {
                        new Secret("hybrid_flow_secret".Sha256())
                    },
                    Scopes = { "scope_used_for_hybrid_flow" },
                    UserClaims = { "role", "admin", "user" }
                },
                new ApiResource("NativeAPI")
                {
                    DisplayName = "Native Client API",
                    ApiSecrets =
                    {
                        new Secret("native_api_secret".Sha256())
                    },
                    Scopes = { "native_api" },
                    UserClaims = { "role", "admin", "user" }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "hybridclient",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret("hybrid_flow_secret".Sha256()) },
                    RequirePkce = false,
                    RedirectUris = { "https://localhost:44381/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44381/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44381/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes = { "openid", "profile", "offline_access",  "scope_used_for_hybrid_flow" }
                },
                new Client
                {
                    ClientId = "native.code",
                    ClientName = "Native Client (Code with PKCE)",

                    RedirectUris = { "https://127.0.0.1:45656" },
                    PostLogoutRedirectUris = { "https://127.0.0.1:45656" },

                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AllowedScopes = { "openid", "profile", "email", "native_api" },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse
                 }
            };
        }
    }
}