using MeBay.Models;
using Microsoft.EntityFrameworkCore;

namespace MeBay.Data
{
    public class MeBayDbContext : DbContext
    {
        public MeBayDbContext(DbContextOptions<MeBayDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasData(
                    new User()
                    {
                        Id = 1,
                        Name = "admin",
                        Email = "admin@fox.hu",
                        Password = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                        Role = "Admin",
                        CreationDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow
                    }
                );
        }
    }
}
