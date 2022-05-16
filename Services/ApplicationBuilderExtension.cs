using ASPShopBag.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPShopBag.Services
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDataBase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var services = scope.ServiceProvider;

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                //Sazdavane na roles
                await SeedRolesAsync(roleManager);
                //sazdavane na SUPER ADMIN s vsi4kite mu roli
                await SeedSuperAdminAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }

            return app;
        }
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            //foreach (var role in Enum.GetValues(Roles))
            //{
            //                    var roleExist = await roleManager.RoleExistsAsync(role); 
            //    if (!roleExist)
            //    { }
            //}
           
                //Seed Roles
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Guest.ToString()));
            }

            public static async Task SeedSuperAdminAsync(UserManager<User> userManager)
            {
                //Seed Default User
                var defaultUser = new User
                {
                    UserName = "superadmin",
                    Email = "superadmin@gmail.com",
                    FullName = "Tonya Belezireva",
                    PhoneNumber = "0899999999",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    var result = await userManager.CreateAsync(defaultUser, "123!@#Qwe");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                        //await userManager.AddToRoleAsync(defaultUser, Roles.Guest.ToString());
                        //await userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());                    
                    }
                }
            }
        }
    }
