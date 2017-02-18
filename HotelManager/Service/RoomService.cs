using HotelManager.Entity;
using System.Collections.Generic;

namespace HotelManager.Service
{
    public interface RoomService
    {

        void Create(Room room);

        Room GetRoom(int id);

        void Edit(Room room);

        Room Delete(int id);

        List<Room> FindRoom(string query);

        List<Room> FindAllRooms();
        
    }
}
