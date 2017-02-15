using HotelManager.Entity;
using System.Collections.Generic;

namespace HotelManager.Dao
{
    public interface RoomDao
    {
        List<Room> FindAll();
    }
}
