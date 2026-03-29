namespace HotelReservation.Interfaces;


public interface ICancellationPolicy
{
    
    decimal CalculateRefund(DateTime checkIn, DateTime cancellationDate, decimal TotalPrice);
}