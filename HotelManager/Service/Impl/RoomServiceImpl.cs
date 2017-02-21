using System.Collections.Generic;
using HotelManager.Entity;
using HotelManager.Dao;

namespace HotelManager.Service.Impl
{
    public class RoomServiceImpl : RoomService
    {

        private RoomDao roomDao;

        public RoomServiceImpl(RoomDao roomDao)
        {
            this.roomDao = roomDao;
        }

        public void Create(Room room)
        {
            roomDao.Save(room);
        }

        public void Edit(Room room)
        {
            roomDao.Save(room);
        }

        public List<Room> FindRoom(string query, bool old)
        {
            if(old)
            {
                return roomDao.FindOld(query);
            }

            return roomDao.Find(query);
        }

        public Room GetRoom(int id)
        {
            return roomDao.Find(id);
        }
    }
}
