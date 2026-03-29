namespace HotelReservation.Services;

using HotelReservation.Models;

// Consumer-owned abstraction for housekeeping notifications.
public interface ICleaningNotifier
{
    void Notify(CleaningTask task);
}
