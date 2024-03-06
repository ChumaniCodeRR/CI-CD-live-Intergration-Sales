using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Integration_Sales_Order_Test.Entities;
using Microsoft.Extensions.Options;


namespace Integration_Sales_Order_Test.Helpers
{
    public class DataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        private readonly IConfiguration Configuration;
      

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DataAccessMySqlProvider"));
        }
    }
}
