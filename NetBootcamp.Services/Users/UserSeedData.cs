using Microsoft.AspNetCore.Identity;
using NetBootcamp.Repository.Identity;
using System.Security.Claims;

namespace NetBootcamp.Services.Users;

public class UserSeedData
{
    public async static Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        var adminRole = await roleManager.FindByNameAsync("admin");
        if (adminRole == null)
        {
            adminRole = new AppRole { Name = "admin" };
            await roleManager.CreateAsync(adminRole);
        }

        var editorRole = await roleManager.FindByNameAsync("editor");
        if (editorRole == null)
        {
            editorRole = new AppRole { Name = "editor" };
            await roleManager.CreateAsync(editorRole);
            editorRole = await roleManager.FindByNameAsync("editor");
        }

        var editorRoleClaims = await roleManager.GetClaimsAsync(editorRole);
        if (!editorRoleClaims.Any())
        {
            await roleManager.AddClaimAsync(editorRole, new Claim("update", "true"));
            await roleManager.AddClaimAsync(editorRole, new Claim("delete", "true"));
        }


        var user = userManager.Users.FirstOrDefault();

        
        if (user is not null && !await userManager.IsInRoleAsync(user, "editor"))
        {
            await userManager.AddToRoleAsync(user, "editor");
        }

    }
}