using AutoMarketHub.WebApi.DTOs;
using FluentValidation;

namespace AutoMarketHub.WebApi.Validators;

public class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
{
    public CreateBookingDtoValidator()
    {
        RuleFor(booking => booking.CarId)
            .GreaterThan(0);

        RuleFor(booking => booking.CustomerName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(120);

        RuleFor(booking => booking.CustomerPhone)
            .NotEmpty()
            .Matches(@"^\+?[0-9\s\-\(\)]{7,20}$")
            .WithMessage("Phone number has invalid format.");

        RuleFor(booking => booking.CustomerEmail)
            .NotEmpty()
            .EmailAddress();

        RuleFor(booking => booking.PreferredDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Preferred date must be in the future.");
    }
}
