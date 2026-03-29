namespace HotelReservation.Infrastructure;

using HotelReservation.Services;

public class FileLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}
