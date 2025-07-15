using Microsoft.EntityFrameworkCore;
using mobibank_test.model;

namespace mobibank_test.db
{

    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<FieldCell> FieldCells { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=db;Port=5432;Database=tic-tat-toe-db;Username=postus;Password=passus");
        }
    }
}