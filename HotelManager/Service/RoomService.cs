using HotelManager.Entity;
using System.Collections.Generic;

namespace HotelManager.Service
{
    public interface RoomService
    {

        Room Create(Room room);
        Room GetRoom(int id);
        void Edit(Room room);
        Room Delete(int id);
        List<Room> FindAllRooms();
        
    }
}
