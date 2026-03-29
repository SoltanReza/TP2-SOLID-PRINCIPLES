namespace HotelReservation.Interfaces;

public class NonRefundableReservation : IReservation
{
    public string Id { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string Status { get; set; } = "Confirmed";
    public decimal TotalPrice { get; set; }

    public decimal CalculateRefund()
    {
        return 0m;
    }
}
