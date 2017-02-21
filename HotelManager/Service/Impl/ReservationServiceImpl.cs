using HotelManager.Dao;
using HotelManager.Entity;
using System.Collections.Generic;

namespace HotelManager.Service.Impl
{
    public class ReservationServiceImpl : ReservationService
    {

        private ReservationDao reservationDao;

        public ReservationServiceImpl(ReservationDao reservationDao)
        {
            this.reservationDao = reservationDao;
        }

        public void Create(Reservation reservation)
        {
            reservationDao.Save(reservation);
        }

        public void Edit(Reservation reservation)
        {
            reservationDao.Save(reservation);
        }

        public List<Reservation> FindCanceledReservation(string query)
        {
            return reservationDao.FindAllCanceled(query);
        }

        public List<Reservation> FindPastReservation(string query)
        {
            return reservationDao.FindAllPast(query);
        }

        public List<Reservation> FindReservation(Room room)
        {
            return reservationDao.FindAll(room);
        }

        public List<Reservation> FindReservation(string query)
        {
            return reservationDao.FindAll(query);
        }

    }
}
