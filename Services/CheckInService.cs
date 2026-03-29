namespace HotelReservation.Services;

using HotelReservation.Infrastructure;
using HotelReservation.Models;

// SRP VIOLATION (Example 2): A single method mixes multiple levels of abstraction.
// High-level business rules sit next to low-level cache manipulation and config reading.
public class CheckInService
{
    private readonly Dictionary<string, CacheEntry> _cache = new();
    private readonly Dictionary<string, Reservation> _dataStore;
    private FileLogger _logger = new FileLogger();
    public CheckInService(Dictionary<string, Reservation> dataStore)
    {
        _dataStore = dataStore;
    }

    public void ProcessCheckIn(Reservation reservation)
    {
     
        // LOW LEVEL: cache manipulation
        if (_cache.ContainsKey(reservation.Id))
            _cache.Remove(reservation.Id);
        _cache[reservation.Id] = new CacheEntry(DateTime.Now, "CheckedIn");


        // LOW LEVEL: direct state mutation
        reservation.Status = "CheckedIn";

        // LOW LEVEL: direct notification
        _logger.Log($"[SMS] Room {reservation.RoomId} is now occupied");
    }

    public void ProcessCheckOut(Reservation reservation)
    {
        
        // LOW LEVEL: cache cleanup
        if (_cache.ContainsKey(reservation.Id))
            _cache.Remove(reservation.Id);

        _logger.Log($"[OK] Room {reservation.RoomId} is now free");
    }
}
