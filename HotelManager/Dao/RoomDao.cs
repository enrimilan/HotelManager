using HotelManager.Entity;
using System.Collections.Generic;

namespace HotelManager.Dao
{
    public interface RoomDao
    {

        void Save(Room room);

        List<Room> FindOld(string query);

        List<Room> Find(string query);

    }
}
