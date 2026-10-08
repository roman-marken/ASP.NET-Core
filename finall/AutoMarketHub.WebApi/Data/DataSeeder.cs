using AutoMarketHub.WebApi.Domain;

namespace AutoMarketHub.WebApi.Data;

public static class DataSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Brands.Any())
        {
            return;
        }

        var toyota = new Brand { Name = "Toyota", Country = "Japan" };
        var bmw = new Brand { Name = "BMW", Country = "Germany" };
        var ford = new Brand { Name = "Ford", Country = "USA" };
        var hyundai = new Brand { Name = "Hyundai", Country = "South Korea" };

        context.Brands.AddRange(toyota, bmw, ford, hyundai);
        context.SaveChanges();

        context.Cars.AddRange(
            new Car
            {
                BrandId = toyota.Id,
                Model = "Camry",
                Year = 2021,
                Price = 24500,
                Mileage = 43000,
                FuelType = "Petrol",
                Transmission = "Automatic",
                IsAvailable = true
            },
            new Car
            {
                BrandId = toyota.Id,
                Model = "RAV4",
                Year = 2022,
                Price = 31900,
                Mileage = 28000,
                FuelType = "Hybrid",
                Transmission = "Automatic",
                IsAvailable = true
            },
            new Car
            {
                BrandId = bmw.Id,
                Model = "320i",
                Year = 2020,
                Price = 27800,
                Mileage = 52000,
                FuelType = "Petrol",
                Transmission = "Automatic",
                IsAvailable = true
            },
            new Car
            {
                BrandId = bmw.Id,
                Model = "X5",
                Year = 2019,
                Price = 41500,
                Mileage = 69000,
                FuelType = "Diesel",
                Transmission = "Automatic",
                IsAvailable = false
            },
            new Car
            {
                BrandId = ford.Id,
                Model = "Focus",
                Year = 2018,
                Price = 12900,
                Mileage = 88000,
                FuelType = "Petrol",
                Transmission = "Manual",
                IsAvailable = true
            },
            new Car
            {
                BrandId = ford.Id,
                Model = "Kuga",
                Year = 2021,
                Price = 22400,
                Mileage = 39000,
                FuelType = "Diesel",
                Transmission = "Automatic",
                IsAvailable = true
            },
            new Car
            {
                BrandId = hyundai.Id,
                Model = "Tucson",
                Year = 2023,
                Price = 33700,
                Mileage = 12000,
                FuelType = "Hybrid",
                Transmission = "Automatic",
                IsAvailable = true
            },
            new Car
            {
                BrandId = hyundai.Id,
                Model = "Elantra",
                Year = 2020,
                Price = 16800,
                Mileage = 61000,
                FuelType = "Petrol",
                Transmission = "Automatic",
                IsAvailable = false
            });

        context.SaveChanges();
    }
}
