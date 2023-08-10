using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace HotelReview.Entities
{
    public class HotelDbContext : DbContext
    {
        string connectionstring =
        "Server=localhost;Database=HotelDb;Trusted_Connection=True;";
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelRooms> HotelRooms { get; set; }
        public DbSet<ParkingPlace> ParkingPlace { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder.Entity<Hotel>(x =>
            {
                x.HasOne(x => x.Address)
                .WithOne(x => x.Hotel)
                .HasForeignKey<Address>(x => x.HotelId);

                x.HasOne(x => x.Parking)
                .WithOne(x => x.Hotel)
                .HasForeignKey<Parking>(x => x.HotelId);

                x.HasMany(x => x.Rooms)
                .WithOne(x => x.Hotel)
                .HasForeignKey(x => x.HotelId);

                x.Property(e => e.HasGym)
                 .IsRequired();
                x.Property(e => e.Name)
                 .IsRequired();
                x.Property(e => e.ReceptionNumber)
                 .IsRequired();
            });

            modelBuilder.Entity<HotelRooms>(entity =>
            {
                entity.Property(e => e.IsAvailable)
                .IsRequired();
                entity.Property(e => e.HowManyBeds)
                .IsRequired();
                entity.Property(e => e.Price)
                .IsRequired();
            });

            modelBuilder.Entity<Address>(x =>
            {
                x.Property(x => x.Street)
                .HasMaxLength(50)
                .IsRequired();

                x.Property(x => x.City)
                .HasMaxLength(50)
                 .IsRequired();
            });

            modelBuilder.Entity<Parking>(x =>
            {
                x.HasMany(x => x.ParkingPlace)
                .WithOne(x => x.Parking)
                .HasForeignKey(x => x.ParkingId);

                x.Property(x => x.AnyPlaces)
                .IsRequired();
            });

            modelBuilder.Entity<ParkingPlace>(entity =>
            {

                entity.Property(e => e.Price)
                .IsRequired();
                entity.Property(e => e.Size)
                .IsRequired();
            });


        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionstring);
        }
    }
}
