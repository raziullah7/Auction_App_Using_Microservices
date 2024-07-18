using System.Security.Claims;
using IdentityModel;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        // creating scope
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        // making db using DbContext
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // apply any migrations that were in the pipeline
        context.Database.Migrate();

        // make a user manager from AspNetCore.Identity
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        // if any users exist previously, don't execute the rest of the code
        if (userMgr.Users.Any()) return;
        
        // check if the username already exists
        var alice = userMgr.FindByNameAsync("alice").Result;
        // if not, define the user
        if (alice == null)
        {
            alice = new ApplicationUser
            {
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
            };
            // create the user in DB
            var result = userMgr.CreateAsync(alice, "Pass123$").Result;
            // if creation in DB fails, then throw exception
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            // list of claims the user made about themselves
            result = userMgr.AddClaimsAsync(alice, new Claim[]{
                new Claim(JwtClaimTypes.Name, "Alice Smith")
            }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("alice created");
        }
        else
        {
            Log.Debug("alice already exists");
        }

        var bob = userMgr.FindByNameAsync("bob").Result;
        if (bob == null)
        {
            bob = new ApplicationUser
            {
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(bob, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(bob, new Claim[]{
                new Claim(JwtClaimTypes.Name, "Bob Smith")
            }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("bob created");
        }
        else
        {
            Log.Debug("bob already exists");
        }
    }
}
