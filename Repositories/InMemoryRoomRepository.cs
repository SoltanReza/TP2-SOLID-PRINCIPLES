namespace HotelReservation.Repositories;

using HotelReservation.Models;
using HotelReservation.Services;

public class InMemoryRoomRepository : IRoomRepository
{
    private static readonly Dictionary<string, Room> _rooms = new()
    {
        { "101", new Room { Id = "101", Type = "Standard", MaxGuests = 2, PricePerNight = 80m } },
        { "102", new Room { Id = "102", Type = "Standard", MaxGuests = 2, PricePerNight = 80m } },
        { "201", new Room { Id = "201", Type = "Suite", MaxGuests = 2, PricePerNight = 200m } },
        { "301", new Room { Id = "301", Type = "Family", MaxGuests = 4, PricePerNight = 120m } }
    };
    private readonly IReservationRepository _reservationRepo = new InMemoryReservationRepository();
   

    public void SeedRooms(List<Room> rooms)
    {
        foreach (var room in rooms)
            _rooms[room.Id] = room;
    }

    public Room? GetById(string roomId)
    {
        return _rooms.TryGetValue(roomId, out var room) ? room : null;
    }

    public List<Room> GetAvailableRooms(DateTime from, DateTime to)
    {
        var reservedRoomIds = _reservationRepo.GetByDateRange(from, to)
            .Select(r => r.RoomId)
            .ToHashSet();

        return _rooms.Values
            .Where(r => !reservedRoomIds.Contains(r.Id))
            .ToList();
    }

    public void Save(Room room)
    {
        _rooms[room.Id] = room;
    }
    public List<Room> GetAll()
    {
        return _rooms.Values.ToList();
    }
}
