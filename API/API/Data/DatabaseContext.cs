using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<Provider> Providers{ get; set; }
        public DbSet<Brand> Brands{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
