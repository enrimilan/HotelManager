using HotelManager.Entity;

namespace HotelManager.Service
{
    interface RoomService
    {

        Room create(Room room);
        Room getRoom(int id);
        void edit(Room room);
        Room delete(int id);

    }
}
