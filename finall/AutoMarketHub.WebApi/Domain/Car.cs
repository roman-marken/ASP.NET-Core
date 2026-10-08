namespace AutoMarketHub.WebApi.Domain;

public class Car
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }
    public int Mileage { get; set; }
    public string FuelType { get; set; } = string.Empty;
    public string Transmission { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }

    public Brand? Brand { get; set; }
    public List<BookingRequest> BookingRequests { get; set; } = new();
}
