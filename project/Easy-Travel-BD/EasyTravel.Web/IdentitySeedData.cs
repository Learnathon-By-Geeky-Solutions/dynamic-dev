using EasyTravel.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EasyTravel.Web
{
    public static class IdentitySeedData
    {
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
                                        .CreateLogger("IdentitySeedData");

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            string roleName = "admin";
            string adminEmail = "admin@easytravel.com";
            string adminPassword = "Admin@123"; // ✅ Your fixed pass-word

            try
            {
                // Create Admin role if it doesn't exist
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new Role
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    };
                    var roleResult = await roleManager.CreateAsync(role);
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                            logger.LogError($"Error creating role '{roleName}': {error.Description}");
                        return;
                    }
                }

                // Create Admin user if it doesn't exist
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var user = new User
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        ProfilePicture = string.Empty
                    };

                    var createResult = await userManager.CreateAsync(user, adminPassword);
                    if (createResult.Succeeded)
                    {
                        var addToRoleResult = await userManager.AddToRoleAsync(user, roleName);
                        if (!addToRoleResult.Succeeded)
                        {
                            foreach (var error in addToRoleResult.Errors)
                                logger.LogError($"Error adding user to role '{roleName}': {error.Description}");
                        }
                        else
                        {
                            logger.LogInformation($"Admin user created and assigned role '{roleName}'.");
                        }
                    }
                    else
                    {
                        foreach (var error in createResult.Errors)
                            logger.LogError($"Error creating admin user: {error.Description}");
                    }
                }
                else
                {
                    logger.LogInformation($"Admin user already exists.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the admin user.");
            }
        }
    }

}
