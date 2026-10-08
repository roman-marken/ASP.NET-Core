using AutoMarketHub.WebApi.DTOs;
using AutoMarketHub.WebApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AutoMarketHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;
    private readonly IValidator<CreateCarDto> _validator;

    public CarsController(ICarService carService, IValidator<CreateCarDto> validator)
    {
        _carService = carService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<CarDto>>> GetCars([FromQuery] CarQueryParameters query)
    {
        var cars = await _carService.GetCarsPagedAsync(query);
        return Ok(cars);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CarDto>> GetCar(int id)
    {
        var car = await _carService.GetByIdAsync(id);

        if (car is null)
        {
            return NotFound();
        }

        return Ok(car);
    }

    [HttpPost]
    public async Task<ActionResult<CarDto>> CreateCar(CreateCarDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
        }

        var car = await _carService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var deleted = await _carService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
