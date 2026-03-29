namespace HotelReservation.Services;

using HotelReservation.Models;
using HotelReservation.Infrastructure;
using HotelReservation.Repositories;

public class ReservationService : IService
{
    private static int _counter = 0;
    private  ILogger _logger ; 
    private IReservationRepository _reservationRepo;
    
    public ReservationService( IReservationRepository reservationRepo, ILogger logger)
    {
        _reservationRepo = reservationRepo;
        _logger = logger;
    }
    
    public Reservation CreateReservation(string guestName, string roomId, DateTime checkIn,
        DateTime checkOut, int guestCount, string roomType, string email , Room room)
    {
        
        _logger.Log($"[LOG] Creating reservation for {guestName}...");

       
       
        var isAvailable = !_reservationRepo.GetAll().Any(r =>
            r.RoomId == roomId &&
            r.Status != "Cancelled" &&
            r.CheckIn < checkOut &&
            r.CheckOut > checkIn);
        if (!isAvailable)
            throw new Exception($"Room {roomId} is not available for {checkIn:dd/MM} -> {checkOut:dd/MM}");

      
        var nights = (checkOut - checkIn).Days;
      
        var total = nights * room.PricePerNight;

        // APPLICATION: create and store
        _counter++;
        var reservation = new Reservation
        {
            Id = $"R-{_counter:D3}",
            GuestName = guestName,
            RoomId = roomId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            GuestCount = guestCount,
            RoomType = roomType,
            Status = "Confirmed",
            Email = email,
            TotalPrice = total
        };
        _reservationRepo.Save(reservation);

        
        _logger.Log($"[LOG] Reservation {reservation.Id} created.");

        return reservation;
    }

    public Reservation? GetReservation(string id)
    {
        return _reservationRepo.GetById(id);
    }

    public List<Reservation> GetAll()
    {
        return _reservationRepo.GetAll();
    }


    public  void Reset()
    {
        _reservationRepo.Clear();
        _counter = 0;
    }
}
