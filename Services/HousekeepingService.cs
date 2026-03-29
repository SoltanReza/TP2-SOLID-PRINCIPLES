namespace HotelReservation.Services;

using HotelReservation.Models;

public class HousekeepingService
{
    private readonly ICleaningNotifier _cleaningNotifier;

    public HousekeepingService(ICleaningNotifier cleaningNotifier)
    {
        _cleaningNotifier = cleaningNotifier;
    }

    public List<CleaningTask> GenerateLinenChangeSchedule(Reservation reservation)
    {
        var tasks = new List<CleaningTask>();
        var current = reservation.CheckIn.AddDays(3);
        while (current < reservation.CheckOut)
        {
            tasks.Add(new CleaningTask
            {
                RoomId = reservation.RoomId,
                Date = current,
                Type = "LinenChange",
                HousekeeperEmail = "housekeeping@masdesoliviers.fr",
                Time = new TimeSpan(10, 0, 0)
            });
            current = current.AddDays(3);
        }
        return tasks;
    }

    public void NotifyHousekeeper(CleaningTask task)
    {
        _cleaningNotifier.Notify(task);
    }
    public List<DateTime> GetLinenChangeDays(DateTime CheckIn, DateTime CheckOut)
    {
        var days = new List<DateTime>();
        var current = CheckIn.AddDays(3);
        while (current < CheckOut)
        {
            days.Add(current);
            current = current.AddDays(3);
        }
        return days;
    }
}
