using AutoMarketHub.WebApi.DTOs;
using AutoMarketHub.WebApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AutoMarketHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IValidator<CreateBookingDto> _validator;

    public BookingsController(IBookingService bookingService, IValidator<CreateBookingDto> validator)
    {
        _bookingService = bookingService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookingDto>>> GetBookings()
    {
        var bookings = await _bookingService.GetAllAsync();
        return Ok(bookings);
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> CreateBooking(CreateBookingDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
        }

        var result = await _bookingService.CreateAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetBookings), new { id = result.Booking!.Id }, result.Booking);
    }
}
