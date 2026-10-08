namespace AutoMarketHub.WebApi.DTOs;

public class BookingCreateResult
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
    public BookingDto? Booking { get; set; }

    public static BookingCreateResult Success(BookingDto booking)
    {
        return new BookingCreateResult
        {
            IsSuccess = true,
            Booking = booking
        };
    }

    public static BookingCreateResult Failed(string error)
    {
        return new BookingCreateResult
        {
            IsSuccess = false,
            Error = error
        };
    }
}
