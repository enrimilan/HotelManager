using HotelManager.Dao.Impl;
using HotelManager.Service.Impl;

namespace HotelManager.Service
{
    public class ServiceFactory
    {

        public static RoomService GetRoomService()
        {
            return new RoomServiceImpl(new RoomDaoImpl());
        }

        public static ReservationService GetReservationService()
        {
            return new ReservationServiceImpl(new ReservationDaoImpl());
        }

    }
}
