namespace HotelReservation.Services;

using HotelReservation.Interfaces;

public class FlexiblePolicy : ICancellationPolicy
{
    public decimal CalculateRefund(DateTime checkIn, DateTime cancellationDate, decimal totalPrice)
    {
        int daysBeforeCheckIn = (checkIn - cancellationDate).Days;

        return daysBeforeCheckIn >= 1 ? totalPrice : 0m;
    }
}