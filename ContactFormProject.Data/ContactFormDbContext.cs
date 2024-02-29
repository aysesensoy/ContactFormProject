using Microsoft.EntityFrameworkCore;

namespace ContactFormProject.Data
{
    public class ContactFormDbContext : DbContext
    {
        private string _connectionString;
        public ContactFormDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Mail> Mails { get; set; }
    }
}
