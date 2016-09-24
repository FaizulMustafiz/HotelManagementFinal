namespace HotelManagementFinal.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HotelManagementSystemContext : DbContext
    {

        public virtual DbSet<CheckIn> CheckIns { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<IdentificationType> IdentificationTypes { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasMany(e => e.CheckIns)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Room>()
                .Property(e => e.RoomPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Room>()
                .HasMany(e => e.CheckIns)
                .WithRequired(e => e.Room)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoomType>()
                .HasMany(e => e.CheckIns)
                .WithRequired(e => e.RoomType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoomType>()
                .HasMany(e => e.Rooms)
                .WithRequired(e => e.RoomType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IdentificationType>()
                .HasMany(e => e.Customers)
                .WithRequired(e => e.IdentificationType)
                .WillCascadeOnDelete(false);
        }
    }
}
