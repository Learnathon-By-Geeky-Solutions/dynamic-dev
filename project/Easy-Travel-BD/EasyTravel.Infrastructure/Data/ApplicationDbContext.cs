using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyTravel.Domain.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EasyTravel.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,IdentityRole<Guid>,Guid>
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Photographer> Photographers { get; set; }
        public DbSet<Guide> Guides { get; set; }
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

            modelBuilder.Entity<Photographer>()
                .Property(a => a.HireDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Photographer>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Photographer>()
              .Property(b => b.Rating)
              .HasColumnType("decimal(3,2)");
            modelBuilder.Entity<Photographer>()
               .Property(a => a.Status)
               .HasDefaultValue("Active");
            modelBuilder.Entity<Guide>()
           .Property(a => a.HireDate)
           .ValueGeneratedOnAdd()
           .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Guide>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Guide>()
              .Property(b => b.Rating)
              .HasColumnType("decimal(3,2)");
            modelBuilder.Entity<Guide>()
                .Property(a => a.Status)
                .HasDefaultValue("Active");

            modelBuilder.Entity<User>()
                .Property(a => a.CreatedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");


            modelBuilder.Entity<IdentityRole<Guid>>()
                .HasData(
                new IdentityRole<Guid>
                {
                    Id = new Guid("b3c9d8f4-1a2b-4c5d-9e6f-7a8b9c0d1e2f"),
                    Name = "admin",
                    NormalizedName = "admin"
                },
                new IdentityRole<Guid>
                {
                    Id = new Guid("f7e6d5c4-b3a2-1f0e-9d8c-7b6a5c4d3e2f"),
                    Name = "client",
                    NormalizedName = "client"
                }
                );
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

            modelBuilder.Entity<Photographer>().HasData(
           new Photographer
           {
               Id = new Guid("2a3d2f79-4f8e-4f87-8a38-41c70f4284b6"),
               FirstName = "Irfan",
               LastName = "Mahmud",
               Email = "irfan.mahmud@example.com",
               ContactNumber = "123-456-7890",
               Address = "123 Main St, Anytown, USA",
               ProfilePicture = "profile.jpg",
               Bio = "Experienced photographer with a passion for Nature photography.",
               DateOfBirth = new DateTime(1985, 5, 15),
               Specialization = "Communication,Hiking,Swimming,Skydive",
               YearsOfExperience = 5,
               Availability = true,
               HourlyRate = 50.00m,
               Status = "Active",
               SocialMediaLinks = "https://twitter.com/johndoe",
               Skills = "Photography,Video Editing,Grahphics Design",
               Rating = 4.5m,
               PortfolioUrl = "https://johndoeportfolio.com",
               AgencyId = new Guid("b8a1d0c5-3f2b-4f8a-9d87-1e4f2e6c1a5b")
           });

            modelBuilder.Entity<Guide>().HasData(
           new Guide
           {
               Id = new Guid("8a9c56d0-8be6-4d34-8e0f-8a7c0a7d9637"),
               FirstName = "Irfan",
               LastName = "Mahmud",
               Email = "irfan.mahmud@example.com",
               ContactNumber = "+1234567890",
               Address = "1234 Main St, City, Country",
               ProfilePicture = "profile-pic.jpg",
               Bio = "Experienced professional with a background in various fields.",
               DateOfBirth = new DateTime(1985, 5, 15),
               LanguagesSpoken = "English, Spanish",
               Specialization = "Communication,Hiking,Swimming,Skydive",
               YearsOfExperience = 5,
               LicenseNumber = "ABC123456",
               Availability = true,
               HourlyRate = 50.00m,
               Rating = 4.5m,
               Status = "Active",
               AgencyId = new Guid("b8a1d0c5-3f2b-4f8a-9d87-1e4f2e6c1a5b")
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