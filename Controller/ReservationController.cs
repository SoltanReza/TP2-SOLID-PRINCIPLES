namespace HotelReservation.Controllers;

using HotelReservation.Models;
using HotelReservation.Services;
public class ReservationController : IController
{
    private readonly ReservationService _service;
    private readonly RoomService roomService;
    public ReservationController(ReservationService service , RoomService roomService)
    {
        _service = service;
        this.roomService = roomService;
    }

    public string CreateReservation(string guestName, string roomId, DateTime checkIn,
        DateTime checkOut, int guestCount, string roomType, string email)
    {
        Room? room = roomService.GetRoomById(roomId);
        if (room == null)
            throw new Exception($"Room {roomId} does not exist");
            
        Reservation reservation = _service.CreateReservation(guestName, roomId, checkIn, checkOut, guestCount, roomType, email , room);
        return reservation.Id;
    }
    
    public Reservation? GetReservation(string id)
    {
        return _service.GetReservation(id);
    }

} 