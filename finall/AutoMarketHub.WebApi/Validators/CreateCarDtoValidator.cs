using AutoMarketHub.WebApi.DTOs;
using FluentValidation;

namespace AutoMarketHub.WebApi.Validators;

public class CreateCarDtoValidator : AbstractValidator<CreateCarDto>
{
    public CreateCarDtoValidator()
    {
        RuleFor(car => car.BrandId)
            .GreaterThan(0);

        RuleFor(car => car.Model)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(car => car.Year)
            .InclusiveBetween(1990, DateTime.UtcNow.Year + 1);

        RuleFor(car => car.Price)
            .GreaterThan(0);

        RuleFor(car => car.Mileage)
            .GreaterThanOrEqualTo(0);

        RuleFor(car => car.FuelType)
            .NotEmpty()
            .MaximumLength(40);

        RuleFor(car => car.Transmission)
            .NotEmpty()
            .MaximumLength(40);
    }
}
