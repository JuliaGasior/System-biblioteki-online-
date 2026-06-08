using Microsoft.AspNetCore.Identity;
using SystemBibliotekiOnline.Models;
using SystemBibliotekiOnline.Models.DbModels;

namespace SystemBibliotekiOnline.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            DatabaseContext context)
        {
            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string adminEmail = "admin@biblioteka.pl";
            string password = "Admin123!";

            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Administrator",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, password);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ",
                        result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            if (!context.Authors.Any())
            {
                context.Authors.AddRange(
                    new Author { Name = "Adam", Surname = "Mickiewicz" },
                    new Author { Name = "Henryk", Surname = "Sienkiewicz" }
                );

                context.SaveChanges();
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Literatura" },
                    new Category { Name = "Powieść" }
                );

                context.SaveChanges();
            }

            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book
                    {
                        Title = "Pan Tadeusz",
                        ISBN = "978000000001",
                        PublicationYear = 1834,
                        AuthorId = 1,
                        CategoryId = 1,
                        IsAvailable = true
                    },
                    new Book
                    {
                        Title = "Quo Vadis",
                        ISBN = "978000000002",
                        PublicationYear = 1896,
                        AuthorId = 2,
                        CategoryId = 2,
                        IsAvailable = true
                    }
                );

                context.SaveChanges();
            }
        }
    }
}