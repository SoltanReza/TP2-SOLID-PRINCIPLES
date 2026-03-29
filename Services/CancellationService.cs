namespace HotelReservation.Services;

using HotelReservation.Models;
using HotelReservation.Interfaces;

public class CancellationService
{
    private readonly ICancellationPolicy _cancellationPolicy;

    public CancellationService(ICancellationPolicy cancellationPolicy)
    {
        _cancellationPolicy = cancellationPolicy;
    }

    public decimal CalculateRefund(Reservation reservation, DateTime now)
    {
        return _cancellationPolicy.CalculateRefund(reservation.CheckIn, now, reservation.TotalPrice);
    }

    public void CancelReservation(Reservation reservation, DateTime now)
    {
        var refund = CalculateRefund(reservation, now);
        reservation.Cancel();
        Console.WriteLine(
            $"[OK] Reservation {reservation.Id} cancelled " +
            $"({reservation.CancellationPolicy} policy: " +
            $"{(refund == reservation.TotalPrice ? "full" : "partial")} refund of {refund:F2} EUR)");
    }
}
