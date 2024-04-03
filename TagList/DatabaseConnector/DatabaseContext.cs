using Microsoft.EntityFrameworkCore;
using TagList.Model;
using static TagList.Startup;


namespace TagList.DatabaseConnector
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = DatabaseConfig.GetConnectionString();
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
