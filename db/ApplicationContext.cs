using Microsoft.EntityFrameworkCore;
using mobibank_test.model;

namespace mobibank_test.db
{
    public class ApplicationContext : DbContext
    {
        public string ConnectionString;

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<FieldCell> FieldCells { get; set; }

        public ApplicationContext(string connectionString)
        {
            ConnectionString = connectionString;

            Database.EnsureCreated();
        }

        public ApplicationContext()
        {
            ConnectionString = "Host=db;Port=5432;Database=tic-tat-toe-db;Username=postus;Password=passus";

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }
    }
}