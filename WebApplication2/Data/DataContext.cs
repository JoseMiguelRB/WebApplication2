using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebApplication2.Data.Entities;

namespace WebApplication2.Data
{

    public class DataContext : IdentityDbContext<User>
    {


        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderDetailTemp> OrderDetailTemps
        {
            get; set;
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
