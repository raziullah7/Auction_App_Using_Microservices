using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("auctionApp", "Auction app full access")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // using client secret to request a token (not recommended)
            // this configuration is just for development
            new Client
            {
                ClientName = "Postman",
                ClientId = "postman",
                ClientSecrets = { new Secret("NotASecret".Sha256()) },
                
                AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                RedirectUris = {"https://www.getpostman.com/oauth2/callback"},
                
                AllowedScopes = {"openid", "profile", "auctionApp"}
            },
            new Client
            {
                ClientName = "nextApp",
                ClientId = "nextApp",
                ClientSecrets = { new Secret("secret".Sha256()) },
                
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                RequirePkce = false,
                
                RedirectUris = {"http://localhost:3000/api/auth/callback/server-id"},
                
                AllowOfflineAccess = true,
                AllowedScopes = {"openid", "profile", "auctionApp"},
                AccessTokenLifetime = 3600*24*30
            }
        };
}
