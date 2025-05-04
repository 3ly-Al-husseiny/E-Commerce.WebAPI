using Domain.Contracts;
using Domain.Models;
using Domain.Models.Identity;
using Domain.Models.Orders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;
using Persistence.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {

        /*
         * You Need to Deal with the DataBase . 
         * Need Object From the DataBase. 
         * E_CommerceDbContext Dependency Injection 
         */

        private readonly E_CommerceDbContext _context;
        private readonly E_CommerceIdentityDbContext _identityContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(E_CommerceDbContext context, E_CommerceIdentityDbContext identityContext,
            UserManager<AppUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _identityContext = identityContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        /*
         * this function is class member method so we will create object in the main.cs to run this function once the program Run.
         */
        public async Task InitializeAsync()
        {
            try
            {

                // (1)Create DataBase If It Doesn't Exist && Apply Any Pending Migrations.

                /*
                 * You Need to Deal with the DataBase . 
                 * Need Object From the DataBase. 
                 * E_CommerceDbContext Dependency Injection 
                 */


                // if there are any Pending migrations
                // GetPendingMigrations() --> Return a Sequence with the Pending Migrations
                if (_context.Database.GetPendingMigrations().Any())
                {
                    // Apply the Pending Migrations && Create The DataBase if it Doesn't Exist.
                    await _context.Database.MigrateAsync();
                }

                // (2)Seed Data if The Database Empty. 

                /*
                 * Seed the ProductBrand && ProdcutType Then the Product
                 * The Product Entity has ProductBrand and ProdcutType so They Should be Seeded Before It. 
                 * 
                 * The Seeding will Be Done if the DataBase is Empty.
                 */


                // Check if the ProductTypes Table in Database is Empty 
                if (!_context.ProductTypes.Any())
                {
                    // Seeding ProductType from Json Files.

                    // 1. Read All Data From Json Files as String.
                    var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\SeedingFiles\types.json");


                    // 2. Convert / Transform String To C# Objects --> List<ProductTypes>
                    // The Data is jsonString (json --> Serialized)
                    // To Convert it from json to any type , We should DeSerialized it 

                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    // 3. Add List<ProductTypes> To Database.
                    // Check if the types is not null or empty

                    if (types is not null && types.Any())
                    {
                        await _context.ProductTypes.AddRangeAsync(types);
                        await _context.SaveChangesAsync();
                    }
                }


                    // Seeding ProductBrand from Json Files.

                    // Check if the Product Brand is Empty

                    if (!_context.ProductBrands.Any())
                    {
                        // 1. Read All Data From Json Files as String.

                        var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\SeedingFiles\brands.json");

                        // 2. Convert / Transform String To C# Objects --> List<ProductTypes>

                        var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                        // 3. Add List<ProductTypes> To Database.

                        if (brandsData is not null && brandsData.Any())
                        {
                            await _context.ProductBrands.AddRangeAsync(brands);
                            await _context.SaveChangesAsync();
                        }
                    }




                    // Seeding Product from Json Files.

                    // Check if the Product Table is Empty 
                    if (!_context.Products.Any())
                    {
                        // Read Data From Json Files. 
                        var productsData = File.ReadAllText(@"..\Infrastructure\Persistence\Data\SeedingFiles\products.json");

                        // Deserialize the jsonString

                        var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                        // Seed the Products in the DataBase. 
                        // Check if the products is null or Empty. 

                        if (products is not null && products.Any())
                        {
                            await _context.Products.AddRangeAsync(products);
                            await _context.SaveChangesAsync();
                        }
                    }

                    if (!_context.DeliveryMethods.Any())
                    {
                        // Read Data From Json Files. 
                        var deliveryData = File.ReadAllText(@"..\Infrastructure\Persistence\Data\SeedingFiles\delivery.json");

                        // Deserialize the jsonString

                        var delivery = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                        // Seed the Products in the DataBase. 
                        // Check if the products is null or Empty. 

                        if (delivery is not null && delivery.Any())
                        {
                            await _context.DeliveryMethods.AddRangeAsync(delivery);
                            await _context.SaveChangesAsync();
                        }
                    }
            }

            catch (Exception)
            {

                throw;
            }

        }

        public async Task InitializeIdentityAsync()
        {
            if (_identityContext.Database.GetPendingMigrations().Any())
            {
                // Apply the Pending Migrations && Create The DataBase if it Doesn't Exist.
                await _identityContext.Database.MigrateAsync();
            }


            // Seeding Roles

            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "SuperAdmin",
                });
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Admin",
                });
            }


            // Seeding SuperAdmin , Admin
            if (!_userManager.Users.Any())
            {
                var superAdminUser = new AppUser()
                {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "01000000000",
                };

                var adminUser = new AppUser()
                {
                   DisplayName = "Admin",
                   Email = "Admin@gmail.com",
                   UserName = "Admin",
                   PhoneNumber = "01000000000",
                };

                await _userManager.CreateAsync(superAdminUser,"P@ssoW0d");
                await _userManager.CreateAsync(adminUser,"P@ssoW0d");

                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}

