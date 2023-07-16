using DeliveryReviewApp.Models;
using Microsoft.EntityFrameworkCore;
using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PackageOwner> PackageOwners { get; set; }
        public DbSet<PackageCategory> PackageCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PackageCategory>()
                .HasKey(pc => new { pc.PackageId, pc.CategoryId });
            modelBuilder.Entity<PackageCategory>()
                .HasOne(p => p.Package)
                .WithMany(pc => pc.PackageCategories)
                .HasForeignKey(p => p.PackageId);
            modelBuilder.Entity<PackageCategory>()
                .HasOne(c => c.Category)
                .WithMany(pc => pc.PackageCategories)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<PackageOwner>()
                .HasKey(po => new { po.PackageId, po.OwnerId });
            modelBuilder.Entity<PackageOwner>()
                .HasOne(p => p.Package)
                .WithMany(po => po.PackageOwners)
                .HasForeignKey(p => p.PackageId);
            modelBuilder.Entity<PackageOwner>()
                .HasOne(p => p.Owner)
                .WithMany(po => po.PackageOwners)
                .HasForeignKey(c => c.OwnerId);
        }

    }
}
