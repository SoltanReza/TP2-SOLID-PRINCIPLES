namespace HotelReservation.Services;

using HotelReservation.Models;

// ISP VIOLATION: Depends on IReservationRepository (9 methods) but only uses
// GetById and GetTotalRevenue.

//Explication de ex 1.3  cette classe est comtable

public class BillingService
{
    private readonly IReservationRepository _repo;

    public BillingService(IReservationRepository repo)
    {
        _repo = repo;
    }

    public decimal GetRevenueForPeriod(DateTime from, DateTime to)
    {
        decimal revenue = _repo.GetTotalRevenue(from, to).Sum(r => this.CalculateTotal(r.CheckOut, r.CheckIn, r.RoomType, r.GuestCount));
        return revenue;
    }
    public decimal CalculateTotal( DateTime checkOut, DateTime checkIn, string roomType, int guestCount)
    {
        var nights = (checkOut - checkIn).Days;
        var pricePerNight = roomType switch
        {
            "Standard" => 80m,
            "Suite" => 200m,
            "Family" => 120m,
            _ => 0m
        };
        var subtotal = nights * pricePerNight;
        var tva = subtotal * 0.10m;
        var touristTax = guestCount * nights * 1.50m;
        return subtotal + tva + touristTax;
    }
      // Actor: ACCOUNTANT — invoice format
    public string GenerateInvoiceLine( string guestName, DateTime checkIn, DateTime checkOut, string roomType, int guestCount)
    {
        return $"{guestName} | {checkIn:dd/MM} -> {checkOut:dd/MM} | {CalculateTotal(checkOut, checkIn, roomType, guestCount):F2} EUR";
    }
}
