using HotelManager.Entity;
using System.Collections.Generic;

namespace HotelManager.Service
{
    public interface RoomService
    {

        void Create(Room room);

        Room GetRoom(int id);

        void Edit(Room room);

        List<Room> FindRoom(string query, bool old);


    }
}
