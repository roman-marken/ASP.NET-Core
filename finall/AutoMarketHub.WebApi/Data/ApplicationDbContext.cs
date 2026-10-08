using AutoMarketHub.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace AutoMarketHub.WebApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<BookingRequest> BookingRequests => Set<BookingRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(brand => brand.Id);
            entity.Property(brand => brand.Name).IsRequired().HasMaxLength(80);
            entity.Property(brand => brand.Country).IsRequired().HasMaxLength(80);

            entity.HasMany(brand => brand.Cars)
                .WithOne(car => car.Brand)
                .HasForeignKey(car => car.BrandId);
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(car => car.Id);
            entity.Property(car => car.Model).IsRequired().HasMaxLength(120);
            entity.Property(car => car.Price).HasPrecision(18, 2);
            entity.Property(car => car.FuelType).IsRequired().HasMaxLength(40);
            entity.Property(car => car.Transmission).IsRequired().HasMaxLength(40);

            entity.HasMany(car => car.BookingRequests)
                .WithOne(booking => booking.Car)
                .HasForeignKey(booking => booking.CarId);
        });

        modelBuilder.Entity<BookingRequest>(entity =>
        {
            entity.HasKey(booking => booking.Id);
            entity.Property(booking => booking.CustomerName).IsRequired().HasMaxLength(120);
            entity.Property(booking => booking.CustomerPhone).IsRequired().HasMaxLength(30);
            entity.Property(booking => booking.CustomerEmail).IsRequired().HasMaxLength(120);
            entity.Property(booking => booking.Status).IsRequired().HasMaxLength(30);
        });
    }
}
