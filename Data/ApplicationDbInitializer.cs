using Microsoft.AspNetCore.Identity;

namespace HMS.Data
{
    public class ApplicationDbInitializer
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Administrator", "BusinessUser" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task SeedDefaultUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Admin user details
            var adminEmail = "admin@example.com"; var adminPassword = "Admin@12345";

            // Check if the admin user already exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                // Create the admin user
                var newAdminUser = new ApplicationUser
                {
                    UserName = adminEmail, Email = adminEmail, EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newAdminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdminUser, "Administrator");
                }
            }
            // Check if the normal user already exists
            var userEmail1 = "user1@example.com"; var userPassword1 = "User1@12345";
            var normalUser1 = await userManager.FindByEmailAsync(userEmail1);
            if (normalUser1 == null)
            {
                // Create the admin user
                var newUser1 = new ApplicationUser
                {
                    UserName = userEmail1, Email = userEmail1, EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newUser1, userPassword1);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser1, "BusinessUser");
                }
            }
            // Check if the normal user already exists
            var userEmail2 = "user2@example.com"; var userPassword2 = "User2@12345";
            var normalUser2 = await userManager.FindByEmailAsync(userEmail2);
            if (normalUser2 == null)
            {
                // Create the admin user
                var newUser2 = new ApplicationUser
                {
                    UserName = userEmail2, Email = userEmail2, EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newUser2, userPassword2);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser2, "BusinessUser");
                }
            }
        }
    }
}
