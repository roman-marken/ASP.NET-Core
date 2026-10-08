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

public class CarServiceTests
{
    [Fact]
    public async Task GetCarsPagedAsync_ShouldFilterByBrandAndPrice()
    {
        await using var context = CreateContext();
        var toyota = new Brand { Name = "Toyota", Country = "Japan" };
        var bmw = new Brand { Name = "BMW", Country = "Germany" };

        context.Cars.AddRange(
            CreateCar(toyota, "Camry", 25000),
            CreateCar(toyota, "RAV4", 33000),
            CreateCar(bmw, "320i", 28000));
        await context.SaveChangesAsync();

        var service = new CarService(context, Mock.Of<ILogger<CarService>>());

        var result = await service.GetCarsPagedAsync(new CarQueryParameters
        {
            BrandId = toyota.Id,
            MinPrice = 20000,
            MaxPrice = 30000
        });

        result.TotalCount.Should().Be(1);
        result.Items.Should().ContainSingle();
        result.Items[0].Model.Should().Be("Camry");
    }

    [Fact]
    public async Task CreateAsync_ShouldSaveNewCar()
    {
        await using var context = CreateContext();
        var brand = new Brand { Name = "Hyundai", Country = "South Korea" };
        context.Brands.Add(brand);
        await context.SaveChangesAsync();

        var service = new CarService(context, Mock.Of<ILogger<CarService>>());

        var dto = new CreateCarDto
        {
            BrandId = brand.Id,
            Model = "Tucson",
            Year = 2023,
            Price = 34000,
            Mileage = 15000,
            FuelType = "Hybrid",
            Transmission = "Automatic",
            IsAvailable = true
        };

        var result = await service.CreateAsync(dto);

        result.Id.Should().BeGreaterThan(0);
        result.BrandName.Should().Be("Hyundai");
        context.Cars.Should().HaveCount(1);
        context.Cars.First().Model.Should().Be("Tucson");
    }

    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static Car CreateCar(Brand brand, string model, decimal price)
    {
        return new Car
        {
            Brand = brand,
            Model = model,
            Year = 2021,
            Price = price,
            Mileage = 30000,
            FuelType = "Petrol",
            Transmission = "Automatic",
            IsAvailable = true
        };
    }
}
