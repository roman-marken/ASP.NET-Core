using AutoMarketHub.WebApi.Data;
using AutoMarketHub.WebApi.Domain;
using AutoMarketHub.WebApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AutoMarketHub.WebApi.Services;

public class CarService : ICarService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CarService> _logger;

    public CarService(ApplicationDbContext context, ILogger<CarService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResult<CarDto>> GetCarsPagedAsync(CarQueryParameters query)
    {
        var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
        var pageSize = query.PageSize < 1 ? 10 : Math.Min(query.PageSize, 50);

        var carsQuery = _context.Cars
            .Include(car => car.Brand)
            .AsQueryable();

        if (query.BrandId.HasValue)
        {
            carsQuery = carsQuery.Where(car => car.BrandId == query.BrandId.Value);
        }

        if (query.MinPrice.HasValue)
        {
            carsQuery = carsQuery.Where(car => car.Price >= query.MinPrice.Value);
        }

        if (query.MaxPrice.HasValue)
        {
            carsQuery = carsQuery.Where(car => car.Price <= query.MaxPrice.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.FuelType))
        {
            carsQuery = carsQuery.Where(car => car.FuelType == query.FuelType);
        }

        if (query.OnlyAvailable)
        {
            carsQuery = carsQuery.Where(car => car.IsAvailable);
        }

        carsQuery = query.SortBy?.ToLower() switch
        {
            "price_asc" => carsQuery.OrderBy(car => car.Price),
            "price_desc" => carsQuery.OrderByDescending(car => car.Price),
            "year_desc" => carsQuery.OrderByDescending(car => car.Year),
            "mileage_asc" => carsQuery.OrderBy(car => car.Mileage),
            _ => carsQuery.OrderBy(car => car.Id)
        };

        var totalCount = await carsQuery.CountAsync();

        var cars = await carsQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(car => MapToDto(car))
            .ToListAsync();

        return new PagedResult<CarDto>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = cars
        };
    }

    public async Task<CarDto?> GetByIdAsync(int id)
    {
        var car = await _context.Cars
            .Include(item => item.Brand)
            .FirstOrDefaultAsync(item => item.Id == id);

        return car is null ? null : MapToDto(car);
    }

    public async Task<CarDto> CreateAsync(CreateCarDto dto)
    {
        var brandExists = await _context.Brands.AnyAsync(brand => brand.Id == dto.BrandId);

        if (!brandExists)
        {
            throw new InvalidOperationException("Brand does not exist.");
        }

        var car = new Car
        {
            BrandId = dto.BrandId,
            Model = dto.Model.Trim(),
            Year = dto.Year,
            Price = dto.Price,
            Mileage = dto.Mileage,
            FuelType = dto.FuelType.Trim(),
            Transmission = dto.Transmission.Trim(),
            IsAvailable = dto.IsAvailable
        };

        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
        _logger.LogInformation("New car was created with id {CarId}.", car.Id);

        return (await GetByIdAsync(car.Id))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(item => item.Id == id);

        if (car is null)
        {
            return false;
        }

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        return true;
    }

    private static CarDto MapToDto(Car car)
    {
        return new CarDto
        {
            Id = car.Id,
            BrandId = car.BrandId,
            BrandName = car.Brand?.Name ?? string.Empty,
            Model = car.Model,
            Year = car.Year,
            Price = car.Price,
            Mileage = car.Mileage,
            FuelType = car.FuelType,
            Transmission = car.Transmission,
            IsAvailable = car.IsAvailable
        };
    }
}
