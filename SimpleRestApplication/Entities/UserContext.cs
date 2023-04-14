using Microsoft.EntityFrameworkCore;

namespace SimpleRestApplication.Entities
{
    public class UserContext : DbContext
    {
        public UserContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();


            optionsBuilder.UseSqlite(configuration.GetConnectionString("MyDbConnection"));
        }

        public DbSet<User> Users { get; set; }

    }
}
