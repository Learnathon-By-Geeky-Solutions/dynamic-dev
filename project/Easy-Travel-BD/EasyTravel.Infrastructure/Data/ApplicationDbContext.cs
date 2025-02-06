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