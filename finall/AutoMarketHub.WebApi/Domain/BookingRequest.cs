namespace AutoMarketHub.WebApi.Domain;

public class BookingRequest
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public DateTime PreferredDate { get; set; }
    public string Status { get; set; } = "Pending";

    public Car? Car { get; set; }
}
