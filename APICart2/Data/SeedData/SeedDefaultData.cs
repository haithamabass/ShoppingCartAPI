using APICart2.Models.AuthModels;
using Microsoft.AspNetCore.Identity;

namespace APICart2.Data.SeedData
{
    public class SeedDefaultData
    {
        public enum Roles
        {
            SuperAdmin,
            Admin,
            User
        }


        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

        }



        public static async Task SeedUsers(UserManager<AppUser> userManager)
        {
            #region defaultUser1
            var defaultUser1 = new AppUser
            {
                UserName = "haitham.abass49@gmail.com",
                Email = "haitham.abass49@gmail.com",
                FirstName = "Haitham",
                LastName = "Abass",
                Address = "maadi",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser1.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser1.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser1, "123456");
                    await userManager.AddToRoleAsync(defaultUser1, Roles.SuperAdmin.ToString());
                }
            }
            #endregion


            #region defaultUser2

            var defaultUser2 = new AppUser
            {
                UserName = "Maged.sobhy50@gmail.com",
                Email = "Maged.sobhy50@gmail.com",
                FirstName = "Maged",
                LastName = "Sobhy",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser2.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser2.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser2, "123456");
                    await userManager.AddToRoleAsync(defaultUser2, Roles.User.ToString());
                }
            }
            #endregion






        }
    }
}
