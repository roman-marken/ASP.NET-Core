using AutoMarketHub.WebApi.DTOs;

namespace AutoMarketHub.WebApi.Services;

public interface ICarService
{
    Task<PagedResult<CarDto>> GetCarsPagedAsync(CarQueryParameters query);
    Task<CarDto?> GetByIdAsync(int id);
    Task<CarDto> CreateAsync(CreateCarDto dto);
    Task<bool> DeleteAsync(int id);
}
