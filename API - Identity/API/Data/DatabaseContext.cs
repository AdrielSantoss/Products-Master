using API.Identity;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class DatabaseContext : IdentityDbContext<UserIdentity, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<Provider> Providers{ get; set; }
        public DbSet<Brand> Brands{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole => {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId}); //n pra n


                userRole.HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                userRole.HasOne(ur => ur.User) //n pra n
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
            });

            modelBuilder.Entity<User>().HasData(new List<User>()
            {
                new User(1, "testeUser", "teste@gmail.com", "teste123"),
            });

            modelBuilder.Entity<Product>().HasData(new List<Product>
            {
                new Product(1, "produtoTeste", 87.5, 1, 1)
            });

            modelBuilder.Entity<Provider>().HasData(new List<Provider>
            {
                new Provider(1, "providerTeste", "cpnjFake")
            });

            modelBuilder.Entity<Brand>().HasData(new List<Brand>
            {
                new Brand(1, "brandTeste")
            }) ;
        }
    }
}
