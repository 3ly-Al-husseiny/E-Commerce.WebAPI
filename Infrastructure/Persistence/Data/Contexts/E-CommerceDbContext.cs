using Domain.Models;
using Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Order = Domain.Models.Orders.Order;

namespace Persistence.Data.Contexts
{
    // CLR will Create the Object From the E_CommerceDbContext
    public class E_CommerceDbContext : DbContext
    {

        // Dependency Injection 
        public E_CommerceDbContext(DbContextOptions<E_CommerceDbContext> options) : base(options)
        {
            
        }


        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Assembly Notes

            //// Assembly.GetExecutingAssembly() --> The Current Assembly 
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //// The Assembly of the Class E_CommerceDbContext 
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(E_CommerceDbContext).Assembly);

            //// The E_CommerceDbContext Can be changed over the time ?! 
            //// So We will Create a reference Class just to refere the Assembly from it , and keep this project safe. 
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Assembly_Reference).Assembly);
            #endregion

        }

    }
}
