namespace HotelReservation.Models;

public interface IReservationRepository
{
    void Add(Reservation reservation);
    Reservation? GetById(string id);
    List<Reservation> GetAll();
    void Update(Reservation reservation);
    void Delete(string id);
    List<Reservation> GetByDateRange(DateTime from, DateTime to);
    List<Reservation> GetTotalRevenue(DateTime from, DateTime to);
    void Save(Reservation reservation);
    public void Clear();
}