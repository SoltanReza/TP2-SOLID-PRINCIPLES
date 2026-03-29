namespace HotelReservation.Services;

using HotelReservation.Interfaces;

public class ModeratePolicy : ICancellationPolicy
{
    public decimal CalculateRefund(DateTime checkIn, DateTime cancellationDate, decimal totalPrice)
    {
        int daysBeforeCheckIn = (checkIn - cancellationDate).Days;

        if (daysBeforeCheckIn >= 5) return totalPrice;
        if (daysBeforeCheckIn >= 2) return totalPrice * 0.5m;
        return 0m;
    }
}