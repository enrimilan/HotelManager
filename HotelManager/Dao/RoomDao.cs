using HotelManager.Entity;
using System.Collections.Generic;

namespace HotelManager.Dao
{
    public interface RoomDao
    {

        void Save(Room room);

        List<Room> FindAll();

        List<Room> Find(string query);

    }
}
