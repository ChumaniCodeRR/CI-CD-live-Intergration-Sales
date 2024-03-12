using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Integration_Sales_Order_Test.Entities;
using Microsoft.Extensions.Options;
using Integration_Sales_Order_Test.Model;
using Integration_Sales_Order_Test.Repository.CategoryServices;


namespace Integration_Sales_Order_Test.Helpers
{
    public class DataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<UserLoginDetails> UserLoginDetails { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<Client> Client { get; set; }

        public DbSet<ItemOrders> ItemsOrders { get; set; }

        //new crud for shipment functionality 
        public DbSet<Branch> Branch { get; set; }
        
        public DbSet<Currency> Currency { get; set; }

        public DbSet<Shipment> Shipment { get; set; }

        public DbSet<ShipmentType> ShipmentType { get; set; }

        public DbSet<Warehouse> Warehouse { get; set; }
        

        private readonly IConfiguration Configuration;


        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DataAccessMySqlProvider"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(x =>
            {
                x.HasKey(y => y.CategoryCode);
            });

            modelBuilder.Entity<ItemOrders>(x =>
            {
                x.HasKey(y => y.OrderNum);
            });

            modelBuilder.Entity<ItemOrders>().ToTable("ItemOrders");

        }
    }
}
