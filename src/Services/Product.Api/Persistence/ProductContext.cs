﻿using Contract.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Product.Api.Entity;
using System;

namespace Product.Api.Persistence
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<ProductCatalog> products { get; set; }
        public DbSet<ProductCategory> categories { get; set; }
        public DbSet<ProductCategoryProduct> productCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductCatalog>().HasIndex(x => x.No)
                .IsUnique();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified ||
                e.State == EntityState.Added ||
                e.State == EntityState.Deleted);

            foreach (var item in modified)
            {
                switch (item.State)
                {
                    case EntityState.Added:
                        if (item.Entity is IDateTracking addedEntity)
                        {
                            addedEntity.CreatedDate = DateTime.UtcNow;
                            item.State = EntityState.Added;
                        }
                        break;
                    case EntityState.Modified:
                        Entry(item.Entity).Property("Id").IsModified = false; // không cho sửa
                        if(item.Entity is IDateTracking modifiedEntity)
                        {
                            modifiedEntity.LassModifiedDate = DateTime.UtcNow;
                            item.State = EntityState.Modified;
                        }
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
                
        }
    }

}
