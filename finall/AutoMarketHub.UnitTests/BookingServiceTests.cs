using AutoMarketHub.WebApi.Data;
using AutoMarketHub.WebApi.Domain;
using AutoMarketHub.WebApi.DTOs;
using AutoMarketHub.WebApi.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AutoMarketHub.UnitTests;

public class BookingServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateBooking_WhenCarIsAvailable()
    {
        await using var context = CreateContext();
        var car = await AddCarAsync(context, isAvailable: true);
        var service = new BookingService(context, Mock.Of<ILogger<BookingService>>());

        var dto = new CreateBookingDto
        {
            CarId = car.Id,
            CustomerName = "Ivan Petrenko",
            CustomerPhone = "+380991112233",
            CustomerEmail = "ivan@test.com",
            PreferredDate = DateTime.UtcNow.AddDays(2)
        };

        var result = await service.CreateAsync(dto);

        result.IsSuccess.Should().BeTrue();
        result.Booking.Should().NotBeNull();
        result.Booking!.Status.Should().Be("Pending");
        context.BookingRequests.Should().HaveCount(1);
    }

    [Fact]
    public async Task CreateAsync_ShouldRejectBooking_WhenCarIsNotAvailable()
    {
        await using var context = CreateContext();
        var car = await AddCarAsync(context, isAvailable: false);
        var service = new BookingService(context, Mock.Of<ILogger<BookingService>>());

        var dto = new CreateBookingDto
        {
            CarId = car.Id,
            CustomerName = "Ivan Petrenko",
            CustomerPhone = "+380991112233",
            CustomerEmail = "ivan@test.com",
            PreferredDate = DateTime.UtcNow.AddDays(2)
        };

        var result = await service.CreateAsync(dto);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("This car is not available for a test drive.");
        context.BookingRequests.Should().BeEmpty();
    }

    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static async Task<Car> AddCarAsync(ApplicationDbContext context, bool isAvailable)
    {
        var brand = new Brand
        {
            Name = "Toyota",
            Country = "Japan"
        };

        var car = new Car
        {
            Brand = brand,
            Model = "Camry",
            Year = 2022,
            Price = 26000,
            Mileage = 25000,
            FuelType = "Petrol",
            Transmission = "Automatic",
            IsAvailable = isAvailable
        };

        context.Cars.Add(car);
        await context.SaveChangesAsync();
        return car;
    }
}
