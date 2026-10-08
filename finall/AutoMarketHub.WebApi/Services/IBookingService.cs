using AutoMarketHub.WebApi.DTOs;

namespace AutoMarketHub.WebApi.Services;

public interface IBookingService
{
    Task<List<BookingDto>> GetAllAsync();
    Task<BookingCreateResult> CreateAsync(CreateBookingDto dto);
}
