using HotelManager.Entity;
using System.Collections.Generic;

namespace HotelManager.Dao
{
    public interface ReservationDao
    {

        void Save(Reservation reservation);

        List<Reservation> FindAll(string query);

        List<Reservation> FindAll(Room room);

        List<Reservation> FindAllCanceled(string query);

        List<Reservation> FindAllPast(string query);
    }
}
