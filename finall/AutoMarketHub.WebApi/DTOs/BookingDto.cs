namespace AutoMarketHub.WebApi.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string CarName { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public DateTime PreferredDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
