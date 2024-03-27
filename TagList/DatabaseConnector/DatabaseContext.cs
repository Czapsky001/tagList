using Microsoft.EntityFrameworkCore;
using TagList.Model;


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
    }
}
