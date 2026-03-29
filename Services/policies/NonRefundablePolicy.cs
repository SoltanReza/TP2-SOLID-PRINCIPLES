namespace HotelReservation.Services;

using HotelReservation.Interfaces;

public class NonRefundablePolicy : ICancellationPolicy
{
    public decimal CalculateRefund(DateTime checkIn, DateTime cancellationDate, decimal totalPrice)
    {
        return 0m;
    }
}