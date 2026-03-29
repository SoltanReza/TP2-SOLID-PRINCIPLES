namespace HotelReservation.Controllers;

using HotelReservation.Infrastructure;
using HotelReservation.Models;
using HotelReservation.Services;
public class CheckInController
{
    private readonly CheckInService _checkInService;
    private readonly decimal lateCheckInFee = 25m;
    private FileLogger _logger = new FileLogger();
    public CheckInController(CheckInService checkInService)
    {
        _checkInService = checkInService;
    }

    public void CheckIn(Reservation reservation)
    {
        // HIGH LEVEL: business rule
        if (reservation.Status != "Confirmed")
            throw new Exception($"Cannot check in: reservation is {reservation.Status}");
     
        if (DateTime.Now.Hour >= 22)
            reservation.TotalPrice += lateCheckInFee;
        _checkInService.ProcessCheckIn(reservation);
        _logger.Log($"[OK] {reservation.GuestName} checked in to Room {reservation.RoomId}");
    }

    public void CheckOut(Reservation reservation)
    {
        if (reservation.Status != "CheckedIn")
            throw new Exception($"Cannot check out: reservation is {reservation.Status}");

        reservation.Status = "CheckedOut";

        _checkInService.ProcessCheckOut(reservation);
        _logger.Log($"[OK] {reservation.GuestName} checked out of Room {reservation.RoomId}");
    }
}