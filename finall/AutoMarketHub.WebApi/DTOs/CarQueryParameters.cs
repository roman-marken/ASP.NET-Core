namespace AutoMarketHub.WebApi.DTOs;

public class CarQueryParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int? BrandId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? FuelType { get; set; }
    public bool OnlyAvailable { get; set; }
    public string? SortBy { get; set; }
}
