using HotelManager.Entity;
using System.Collections.Generic;

namespace HotelManager.Service
{
    public interface ReservationService
    {

        void Create(Reservation reservation);

        void Edit(Reservation reservation);

        List<Reservation> FindReservation(Room room);

        List<Reservation> FindReservation(string query);

        List<Reservation> FindPastReservation(string query);

        List<Reservation> FindCanceledReservation(string query);
        
    }
}
