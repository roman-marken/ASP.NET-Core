using AutoMarketHub.WebApi.Data;
using AutoMarketHub.WebApi.Services;
using AutoMarketHub.WebApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("AutoMarketHubDb"));

builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCarDtoValidator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DataSeeder.Seed(dbContext);
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
