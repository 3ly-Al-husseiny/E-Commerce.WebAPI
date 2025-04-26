using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;
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

        public DbInitializer(E_CommerceDbContext context)
        {
            _context = context;
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


                }
            }

            catch (Exception)
            {

                throw;
            }

        }
    }
}

