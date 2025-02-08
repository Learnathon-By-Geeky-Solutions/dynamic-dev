using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyTravel.Domain.Entites;

namespace EasyTravel.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {


        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public DbSet<User> Users { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bus>()
              .Property(b => b.Price)
              .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Agency>()
                .Property(a => a.AddDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Agency>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Agency>().HasData(
            new Agency
            {
                Id = new Guid("b8a1d0c5-3f2b-4f8a-9d87-1e4f2e6c1a5b"),
                Name = "Irfan",
                Address = "Mirpur",
                ContactNumber = "01723695124",
                Website = "irfan.com",
                LicenseNumber = "365981"
            });
        }
        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString, (x) => x.MigrationsAssembly(_migrationAssembly));
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}