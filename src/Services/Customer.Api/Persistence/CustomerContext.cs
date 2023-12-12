using Customer.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Persistence
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
        }

        public DbSet<Entities.Customer> customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Entities.Customer>().HasIndex(x => x.UserName)
                .IsUnique();
            modelBuilder.Entity<Entities.Customer>().HasIndex(x => x.Email)
                .IsUnique();
        }

    }
}
