using AutoMarketHub.WebApi.Data;
using AutoMarketHub.WebApi.Domain;
using AutoMarketHub.WebApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AutoMarketHub.WebApi.Services;

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BookingService> _logger;

    public BookingService(ApplicationDbContext context, ILogger<BookingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<BookingDto>> GetAllAsync()
    {
        return await _context.BookingRequests
            .Include(booking => booking.Car)
            .ThenInclude(car => car!.Brand)
            .OrderByDescending(booking => booking.PreferredDate)
            .Select(booking => MapToDto(booking))
            .ToListAsync();
    }

    public async Task<BookingCreateResult> CreateAsync(CreateBookingDto dto)
    {
        var car = await _context.Cars
            .Include(item => item.Brand)
            .FirstOrDefaultAsync(item => item.Id == dto.CarId);

        if (car is null)
        {
            return BookingCreateResult.Failed("Car was not found.");
        }

        if (!car.IsAvailable)
        {
            _logger.LogInformation("Booking rejected because car {CarId} is not available.", car.Id);
            return BookingCreateResult.Failed("This car is not available for a test drive.");
        }

        var booking = new BookingRequest
        {
            CarId = dto.CarId,
            CustomerName = dto.CustomerName.Trim(),
            CustomerPhone = dto.CustomerPhone.Trim(),
            CustomerEmail = dto.CustomerEmail.Trim(),
            PreferredDate = dto.PreferredDate,
            Status = "Pending"
        };

        _context.BookingRequests.Add(booking);
        await _context.SaveChangesAsync();

        booking.Car = car;
        return BookingCreateResult.Success(MapToDto(booking));
    }

    private static BookingDto MapToDto(BookingRequest booking)
    {
        var carName = booking.Car is null
            ? string.Empty
            : $"{booking.Car.Brand?.Name} {booking.Car.Model}".Trim();

        return new BookingDto
        {
            Id = booking.Id,
            CarId = booking.CarId,
            CarName = carName,
            CustomerName = booking.CustomerName,
            CustomerPhone = booking.CustomerPhone,
            CustomerEmail = booking.CustomerEmail,
            PreferredDate = booking.PreferredDate,
            Status = booking.Status
        };
    }
}
