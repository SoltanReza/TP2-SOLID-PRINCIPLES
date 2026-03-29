namespace HotelReservation.Services;

using HotelReservation.Interfaces;

public class StrictPolicy : ICancellationPolicy
{
    public decimal CalculateRefund(DateTime checkIn, DateTime cancellationDate, decimal totalPrice)
    {
        int daysBeforeCheckIn = (checkIn - cancellationDate).Days;

        if (daysBeforeCheckIn >= 14) return totalPrice;
        if (daysBeforeCheckIn >= 7) return totalPrice * 0.5m;
        return 0m;
    }
}