using HotelReservation.Models;
using HotelReservation.Services;
using HotelReservation.Interfaces;
using HotelReservation.Events;
using HotelReservation.Repositories;
using HotelReservation.Controllers;
using HotelReservation.Infrastructure;

Console.WriteLine("=== Le Mas des Oliviers - Hotel Management System ===");
Console.WriteLine();

Console.WriteLine("--- Scenario 1: Creating Reservations ---");

var reservationController = new ReservationController(
    new ReservationService(
        new InMemoryReservationRepository(),
        new FileLogger()
    ),
    new RoomService(
        new InMemoryRoomRepository(),
        new InMemoryReservationRepository(),
        new FileLogger() 
    )
);

var id1 = reservationController.CreateReservation(
    "Alice Martin", "101", new DateTime(2025, 6, 15), new DateTime(2025, 6, 18),
    2, "Standard", "alice.martin@email.com");
Console.WriteLine($"[OK] Reservation {id1}: Alice Martin, Room 101 (Standard), 15/06 -> 18/06, 2 guests");

var id2 = reservationController.CreateReservation(
    "Bob Dupont", "201", new DateTime(2025, 6, 15), new DateTime(2025, 6, 22),
    2, "Suite", "bob.dupont@email.com");
Console.WriteLine($"[OK] Reservation {id2}: Bob Dupont, Room 201 (Suite), 15/06 -> 22/06, 2 guests");

var id3 = reservationController.CreateReservation(
    "Famille Durand", "301", new DateTime(2025, 6, 20), new DateTime(2025, 6, 25),
    4, "Family", "durand@email.com");
Console.WriteLine($"[OK] Reservation {id3}: Famille Durand, Room 301 (Family), 20/06 -> 25/06, 4 guests");

Console.WriteLine();

Console.WriteLine("--- Scenario 2: Double Booking Attempt ---");
try
{
    reservationController.CreateReservation(
        "Charlie Test", "101", new DateTime(2025, 6, 16), new DateTime(2025, 6, 19),
        2, "Standard", "charlie@email.com");
}
catch (Exception ex)
{
    Console.WriteLine($"[CONFLICT] {ex.Message}");
}
Console.WriteLine();

Console.WriteLine("--- Scenario 3: Cancellation ---");

var cancellationService = new CancellationService(new FlexiblePolicy());
var aliceReservation = reservationController.GetReservation(id1)!;
aliceReservation.CancellationPolicy = "Flexible";
cancellationService.CancelReservation(aliceReservation, new DateTime(2025, 6, 10));
Console.WriteLine();

Console.WriteLine("--- Scenario 4: Check-In / Check-Out ---");
var bobReservation = reservationController.GetReservation(id2)!;
var checkInService = new CheckInController(new CheckInService(new Dictionary<string, Reservation>()));
checkInService.CheckIn(bobReservation);
Console.WriteLine($"[OK] {bobReservation.GuestName} checked in to Room {bobReservation.RoomId}");
checkInService.CheckOut(bobReservation);
Console.WriteLine($"[OK] {bobReservation.GuestName} checked out from Room {bobReservation.RoomId}");
Console.WriteLine();

Console.WriteLine("--- Scenario 5: Billing ---");
var invoiceGenerator = new InvoiceGenerator();

var bobForBilling = new Reservation
{
    Id = id2,
    GuestName = "Bob Dupont",
    RoomId = "201",
    CheckIn = new DateTime(2025, 6, 15),
    CheckOut = new DateTime(2025, 6, 22),
    GuestCount = 2,
    RoomType = "Suite",
    Status = "CheckedOut"
};
var invoice = invoiceGenerator.Generate(bobForBilling);
invoiceGenerator.PrintInvoice(invoice, bobForBilling);
Console.WriteLine();

Console.WriteLine("--- Scenario 6: Housekeeping Schedule ---");
HousekeepingService housekeepingService = new HousekeepingService(
    new EmailCleaningNotifier(new EmailSender()));
var bobForHousekeeping = new Reservation
{
    Id = id2,
    GuestName = "Bob Dupont",
    RoomId = "201",
    CheckIn = new DateTime(2025, 6, 15),
    CheckOut = new DateTime(2025, 6, 22),
    GuestCount = 2,
    RoomType = "Suite"
};
var bobLinenDays = housekeepingService.GetLinenChangeDays(bobForHousekeeping.CheckIn, bobForHousekeeping.CheckOut);
Console.WriteLine($"Linen change schedule for Bob Dupont (Room 201, 15/06 -> 22/06):");
foreach (var day in bobLinenDays)
    Console.WriteLine($"  - {day:dd/MM/yyyy}");

var durandForHousekeeping = new Reservation
{
    Id = id3,
    GuestName = "Famille Durand",
    RoomId = "301",
    CheckIn = new DateTime(2025, 6, 20),
    CheckOut = new DateTime(2025, 6, 25),
    GuestCount = 4,
    RoomType = "Family"
};
var durandLinenDays = housekeepingService.GetLinenChangeDays(durandForHousekeeping.CheckIn, durandForHousekeeping.CheckOut);
Console.WriteLine($"Cleaning tasks for Famille Durand (Room 301, 20/06 -> 25/06):");
foreach (var day in durandLinenDays)
    Console.WriteLine($"  - {day:dd/MM/yyyy}");
Console.WriteLine();

Console.WriteLine("--- Scenario 7: Event Dispatching ---");
var dispatcher = new ReservationEventDispatcher();
dispatcher.Register(new EmailConfirmationHandler());
dispatcher.Register(new HousekeepingSetupHandler());
dispatcher.Dispatch(new ReservationCreatedEvent
{
    ReservationId = "R-001",
    GuestName = "Alice Martin",
    RoomId = "101",
    Email = "alice.martin@email.com",
    CheckIn = new DateTime(2025, 6, 15),
    CheckOut = new DateTime(2025, 6, 18)
});
Console.WriteLine();

Console.WriteLine("--- Scenario 8: Pricing with Seasonal Surcharge ---");
var aliceForPricing = new Reservation
{
    GuestName = "Alice Martin",
    RoomId = "101",
    CheckIn = new DateTime(2025, 6, 15),
    CheckOut = new DateTime(2025, 6, 18),
    RoomType = "Standard"
};

IPriceCalculator baseCalculator = new BasePriceCalculator();
var basePrice = baseCalculator.Calculate(aliceForPricing);
Console.WriteLine($"Base price for Alice (3 nights Standard): {basePrice:F2} EUR");

IPriceCalculator withSurcharge = new SeasonalSurchargeDecorator(baseCalculator, 0.20m);
var surchargedPrice = withSurcharge.Calculate(aliceForPricing);
Console.WriteLine($"With 20% summer surcharge: {surchargedPrice:F2} EUR");
Console.WriteLine();

Console.WriteLine("--- Scenario 9: LSP Demo ---");
ICancellableReservation flexibleRes = new FlexibleReservation
{
    Id = "FLEX-001", GuestName = "Test Flexible", TotalPrice = 200m
};
flexibleRes.Cancel();
Console.WriteLine($"[OK] Flexible reservation cancelled, refund: {flexibleRes.CalculateRefund():F2} EUR");

IReservation nonRefundableRes = new NonRefundableReservation
{
    Id = "NR-001", GuestName = "Test NonRefundable", TotalPrice = 200m
};
Console.WriteLine($"[OK] Non-refundable reservation refund: {nonRefundableRes.CalculateRefund():F2} EUR");
Console.WriteLine();

Console.WriteLine("=== End of Demo ===");
