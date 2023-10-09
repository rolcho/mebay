using MeBay.Models;
using Microsoft.EntityFrameworkCore;

namespace MeBay.Data
{
    public class MeBayDbContext : DbContext
    {
        public MeBayDbContext(DbContextOptions<MeBayDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Item>();
        }
    }
}
