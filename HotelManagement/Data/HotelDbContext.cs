using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
        {
        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HotelDatabase;Integrated Security=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>()
                .HasIndex(h => h.Name)
                .IsUnique();

            modelBuilder.Entity<HotelRoom>()
                .HasOne(hr => hr.Hotel)
                .WithMany(h => h.HotelRooms)
                .HasForeignKey(hr => hr.HotelId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
