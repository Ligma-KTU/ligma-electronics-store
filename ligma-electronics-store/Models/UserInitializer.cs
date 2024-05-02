using Microsoft.AspNetCore.Identity;

namespace ligma_electronics_store.Models
{
    public class UserInitializer
    {
        public static async Task Initialize(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var adminUser = await userManager.FindByNameAsync("admin");

            if (adminUser == null)
            {
                var admin = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@admin.com"
                };

                var result = await userManager.CreateAsync(admin, "Students1!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
