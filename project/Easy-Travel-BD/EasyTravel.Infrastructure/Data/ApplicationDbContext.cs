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
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        private readonly string? _connectionString;
        private readonly string? _migrationAssembly;
        private const string DefaultSqlGetDate = "GETDATE()";
        private const string SqlConlumnType = "decimal(18,2)";
        private const string SqlDefaultIdValue = "NEWID()";
        private const string SqlDefaultStatusValue = "Active";
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Photographer> Photographers { get; set; }
        public DbSet<Guide> Guides { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<HotelBooking> HotelBookings { get; set; }
        public DbSet<PhotographerBooking> PhotographerBookings { get; set; }
        public DbSet<GuideBooking> GuideBookings { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<BusBooking> BusBookings { get; set; }
        public DbSet<CarBooking> CarBookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Payment 
            builder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentMethod)
                .HasConversion<string>();
                entity.Property(e => e.PaymentStatus)
                .HasConversion<string>();
                entity.HasOne(p => p.Booking)
                 .WithMany(b => b.Payments)
                 .HasForeignKey(p => p.BookingId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Booking
            builder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.BookingStatus)
                    .HasConversion<string>();
                entity.Property(e => e.BookingTypes)
                    .HasConversion<string>();
                entity.Property(e => e.CreatedAt)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql(DefaultSqlGetDate);
                entity.Property(e => e.UpdatedAt)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql(DefaultSqlGetDate);
                entity.Property(b => b.TotalAmount)
                .HasColumnType(SqlConlumnType);
                entity.HasOne(u => u.User)
                    .WithMany(b => b.Bookings)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // BusBooking
            builder.Entity<BusBooking>(entity =>
            {
                entity.HasOne(p => p.Bus)
                .WithMany(b => b.BusBookings)
                .HasForeignKey(p => p.BusId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(c => c.Booking)
               .WithOne(b => b.BusBooking)
               .HasForeignKey<BusBooking>(c => c.Id)
               .OnDelete(DeleteBehavior.Cascade);
                entity.ToTable("BusBookings");
            });

            // GuideBooking
            builder.Entity<GuideBooking>(entity =>
            {
                entity.Property(a => a.Id)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql(SqlDefaultIdValue);
                entity.HasOne(p => p.Booking)
                 .WithOne(b => b.GuideBooking)
                 .HasForeignKey<GuideBooking>(p => p.Id)
                 .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(p => p.Guide)
                 .WithMany(b => b.GuideBookings)
                 .HasForeignKey(p => p.GuideId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // PhotographerBooking
            builder.Entity<PhotographerBooking>(entity =>
            {
                entity.Property(a => a.Id)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql(SqlDefaultIdValue);
                entity.HasOne(p => p.Booking)
                    .WithOne(b => b.PhotographerBooking)
                    .HasForeignKey<PhotographerBooking>(p => p.Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(p => p.Photographer)
                      .WithMany(b => b.PhotographerBookings)
                      .HasForeignKey(p => p.PhotographerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // CarBooking
            builder.Entity<CarBooking>(entity =>
            {
                entity.HasOne(p => p.Car)
                  .WithMany(b => b.CarBookings)
                  .HasForeignKey(p => p.CarId)
                  .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(c => c.Booking)
                .WithOne(b => b.CarBooking)
                .HasForeignKey<CarBooking>(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);
            });

            // Seat
            builder.Entity<Seat>()
                .ToTable("Seats");

            // Bus
            builder.Entity<Bus>(entity =>
            {
                entity.HasMany(b => b.Seats)
                   .WithOne(s => s.Bus)
                   .HasForeignKey(s => s.BusId)
                   .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(b => b.BusBookings)
                    .WithOne(bb => bb.Bus)
                    .HasForeignKey(bb => bb.BusId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Property(b => b.Price)
                    .HasColumnType(SqlConlumnType);
            });

            // Agency
            builder.Entity<Agency>(entity =>
            {
                entity.Property(a => a.AddDate)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql(DefaultSqlGetDate);
                entity.Property(a => a.Id)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql(SqlDefaultIdValue);
                entity.HasMany(b => b.Photographers)
                    .WithOne(bb => bb.Agency)
                    .HasForeignKey(bb => bb.AgencyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Photographer
            builder.Entity<Photographer>(entity =>
            {
                entity.ToTable("Photographers");
                entity.Property(a => a.HireDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql(DefaultSqlGetDate);
                entity.Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql(SqlDefaultIdValue);
                entity.Property(b => b.Rating)
              .HasColumnType("decimal(3,2)");
                entity.Property(a => a.Status)
               .HasDefaultValue(SqlDefaultStatusValue);
                entity.Property(p => p.HourlyRate)
              .HasColumnType(SqlConlumnType);
            });

            // Guide
            builder.Entity<Guide>(entity =>
            {
                entity.ToTable("Guides");
                entity.Property(a => a.HireDate)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql(DefaultSqlGetDate);
                entity.Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql(SqlDefaultIdValue);
                entity.Property(b => b.Rating)
              .HasColumnType("decimal(3,2)");
                entity.Property(a => a.Status)
                .HasDefaultValue(SqlDefaultStatusValue);
                entity.Property(g => g.HourlyRate)
                .HasColumnType(SqlConlumnType);

            });
                
            builder.Entity<User>(entity =>
            {
                entity.Property(a => a.CreatedAt)
               .ValueGeneratedOnAdd()
               .HasDefaultValueSql(DefaultSqlGetDate);
            });

            // UserRole
            builder.Entity<IdentityUserRole<Guid>>()
                .Property(a => a.RoleId)
                .ValueGeneratedOnAdd()
                .HasDefaultValue(new Guid("f7e6d5c4-b3a2-1f0e-9d8c-7b6a5c4d3e2f"));

            // Roles
            builder.Entity<Role>()
                .HasData(
                new Role
                {
                    Id = new Guid("b3c9d8f4-1a2b-4c5d-9e6f-7a8b9c0d1e2f"),
                    Name = "admin",
                    NormalizedName = "admin"
                },
                new Role
                {
                    Id = new Guid("f7e6d5c4-b3a2-1f0e-9d8c-7b6a5c4d3e2f"),
                    Name = "client",
                    NormalizedName = "client"
                },
                 new Role
                 {
                     Id = new Guid("4558E034-03AF-4D30-819F-9A24CB81C942"),
                     Name = "agencyManager",
                     NormalizedName = "agencyManager"
                 },
                  new Role
                  {
                      Id = new Guid("C10F83B0-9008-468B-B931-5E73FF416337"),
                      Name = "busManager",
                      NormalizedName = "busManager"
                  },
                   new Role
                   {
                       Id = new Guid("862B8016-7786-4CF2-BCB1-A4AAC017FF2C"),
                       Name = "carManager",
                       NormalizedName = "carManager"
                   },
                   new Role
                   {
                       Id = new Guid("292DCAF2-AADC-493A-8F19-E7905AB98299"),
                       Name = "hotelManager",
                       NormalizedName = "hotelManager"
                   }
                );


            builder.Entity<Agency>().HasData(
            new Agency
            {
                Id = new Guid("b8a1d0c5-3f2b-4f8a-9d87-1e4f2e6c1a5b"),
                Name = "Irfan",
                Address = "Mirpur",
                ContactNumber = "01723695124",
                Website = "irfan.com",
                LicenseNumber = "365981"
            });

            builder.Entity<Photographer>().HasData(
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
               DateOfBirth = new DateTime(2025, 2, 16, 19, 31, 26, DateTimeKind.Utc),
               Specialization = "Communication,Hiking,Swimming,Skydive",
               YearsOfExperience = 5,
               Availability = true,
               HourlyRate = 50.00m,
               PreferredEvents = "marriage",
               PreferredLocations = "dhaka,sylhet",
               Status = SqlDefaultStatusValue,
               SocialMediaLinks = "https://twitter.com/johndoe",
               Skills = "Photography,Video Editing,Grahphics Design",
               Rating = 4.5m,
               PortfolioUrl = "https://johndoeportfolio.com",
               AgencyId = new Guid("b8a1d0c5-3f2b-4f8a-9d87-1e4f2e6c1a5b")
           });

            builder.Entity<Guide>().HasData(
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
               DateOfBirth = new DateTime(2025, 2, 16, 19, 31, 26, DateTimeKind.Utc),
               LanguagesSpoken = "English, Spanish",
               Specialization = "Communication,Hiking,Swimming,Skydive",
               PreferredLocations = "dhaka,sylhet",
               PreferredEvents = "citytour,museumtour,hilltracking",
               YearsOfExperience = 5,
               LicenseNumber = "ABC123456",
               Availability = true,
               HourlyRate = 50.00m,
               Rating = 4.5m,
               Status = SqlDefaultStatusValue,
               AgencyId = new Guid("b8a1d0c5-3f2b-4f8a-9d87-1e4f2e6c1a5b")
           });

            #region Hotel Booking Realated 
            builder.Entity<HotelBooking>()
                .ToTable("HotelBookings");
            builder.Entity<Room>()
             .ToTable("Rooms");
            builder.Entity<Hotel>(entity =>
            {
                // Configure primary key
                entity.HasKey(e => e.Id);

                // Configure properties
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Address)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Description);

                entity.Property(e => e.City)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Phone)
                      .HasMaxLength(15) // Assuming a max length for phone numbers
                      .HasColumnType("varchar(15)");

                entity.Property(e => e.Email)
                      .HasMaxLength(100) // Assuming a max length for email addresses
                      .HasColumnType("varchar(100)");

                entity.Property(e => e.Rating)
                      .IsRequired()
                      .HasDefaultValue(3)
                      .HasColumnType("int");

                entity.Property(e => e.Image)
                      .HasMaxLength(200); // Assuming a max length for image URLs

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql(DefaultSqlGetDate);

                entity.Property(e => e.UpdatedAt)
                      .HasDefaultValueSql(DefaultSqlGetDate);

                // Configure relationships
                entity.HasMany(e => e.Rooms)
                      .WithOne(r => r.Hotel)
                      .HasForeignKey(r => r.HotelId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Room
            builder.Entity<Room>(entity =>
            {
                // Configure primary key
                entity.HasKey(e => e.Id);

                // Configure properties
                entity.Property(e => e.RoomNumber)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.RoomType)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.PricePerNight)
                      .IsRequired()
                      .HasColumnType(SqlConlumnType);

                entity.Property(e => e.MaxOccupancy)
                      .IsRequired();

                entity.Property(e => e.Description);

                entity.Property(e => e.Image1)
                      .HasMaxLength(200);

                entity.Property(e => e.Image2)
                      .HasMaxLength(200);

                entity.Property(e => e.Image3)
                      .HasMaxLength(200);

                entity.Property(e => e.Image4)
                      .HasMaxLength(200);

                entity.Property(e => e.IsAvailable)
                      .IsRequired()
                      .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql(DefaultSqlGetDate);

                entity.Property(e => e.UpdatedAt)
                      .HasDefaultValueSql(DefaultSqlGetDate);

                // Configure relationships
                entity.HasOne(e => e.Hotel)
                      .WithMany(h => h.Rooms)
                      .HasForeignKey(e => e.HotelId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<HotelBooking>(entity =>
            {
                entity.HasOne(p => p.Booking)
                 .WithOne(b => b.HotelBooking)
                 .HasForeignKey<HotelBooking>(p => p.Id)
                 .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Hotel)
                      .WithMany(h => h.HotelBookings)
                      .HasForeignKey(e => e.HotelId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Configure properties
                entity.Property(e => e.CheckInDate)
                      .IsRequired();

                entity.Property(e => e.CheckOutDate)
                      .IsRequired();

                entity.Property(e => e.RoomIdsJson)
                      .IsRequired();
            });


            #endregion Hotel Booking
            // Seed data for Hotel

            builder.Entity<Hotel>().HasData(
       new Hotel
       {
           Id = new Guid("e2a1d0c5-3f2b-4f8a-9d87-1e4f2e6c1a5b"),
           Name = "Grand Hotel",
           Address = "123 Main St, Anytown, USA",
           Description = "A luxurious hotel with all modern amenities.",
           City = "Anytown",
           Phone = "123-456-7890",
           Email = "info@grandhotel.com",
           Rating = 5,
           Image = "grandhotel.jpg",
           CreatedAt = new DateTime(2025, 2, 16, 19, 31, 26, DateTimeKind.Utc),
           UpdatedAt = new DateTime(2025, 2, 16, 19, 31, 26, DateTimeKind.Utc)
       },
       new Hotel
       {
           Id = new Guid("f3a1d0c5-3f2b-4f8a-9d87-1e4f2e6c1a5b"),
           Name = "Sunset Resort",
           Address = "456 Beach Rd, Seaside, USA",
           Description = "A beautiful resort with stunning sunset views.",
           City = "Seaside",
           Phone = "987-654-3210",
           Email = "info@sunsetresort.com",
           Rating = 4,
           Image = "sunsetresort.jpg",
           CreatedAt = new DateTime(2025, 2, 16, 19, 31, 26, DateTimeKind.Utc),
           UpdatedAt = new DateTime(2025, 2, 16, 19, 31, 26, DateTimeKind.Utc)
       }
   );

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
                optionsBuilder.UseSqlServer(_connectionString, x => x.MigrationsAssembly(_migrationAssembly));
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}