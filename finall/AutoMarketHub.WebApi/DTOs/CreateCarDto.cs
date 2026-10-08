namespace AutoMarketHub.WebApi.DTOs;

public class CreateCarDto
{
    public int BrandId { get; set; }
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }
    public int Mileage { get; set; }
    public string FuelType { get; set; } = string.Empty;
    public string Transmission { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
}
