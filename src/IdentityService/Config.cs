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
                AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                ClientName = "Postman",
                
                RedirectUris = {"https://www.getpostman.com/oauth2/callback"},
                
                ClientId = "postman",
                ClientSecrets = { new Secret("NotASecret".Sha256()) },
                AllowedScopes = {"openid", "profile", "auctionApp"}
            }
        };
}
