using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyFinalProject.Models;

namespace MyFinalProject.Data
{
    public class ShareDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Bought> Bought { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Sold> Sold { get; set; }
        public DbSet<Company> Companies { get; set; }

        public ShareDbContext(DbContextOptions<ShareDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Bought>().ToTable("Bought");
            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUser");
            modelBuilder.Entity<Feedback>().ToTable("Feedback");
            modelBuilder.Entity<Sold>().ToTable("Sold");
            modelBuilder.Entity<Company>().ToTable("Companies");
        }
    }
}
