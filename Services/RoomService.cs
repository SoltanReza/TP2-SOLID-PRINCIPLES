namespace HotelReservation.Services;

using HotelReservation.Infrastructure;
using HotelReservation.Models;
using HotelReservation.Repositories;

public class RoomService
{
    private readonly IRoomRepository _roomRepo ;
    private readonly IReservationRepository _reservationRepo ;
    private readonly ILogger _logger;

    public RoomService(IRoomRepository roomRepo, IReservationRepository reservationRepo, ILogger logger)
    {
        _roomRepo = roomRepo;
        _reservationRepo = reservationRepo;
        _logger = logger;
    }
   

    public List<Room> GetAvailableRooms(DateTime from, DateTime to)
    {
        return _roomRepo.GetAvailableRooms(from, to);
    }

    public Room? GetRoomById(string roomId)
    {
        return _roomRepo.GetById(roomId);
    }
}