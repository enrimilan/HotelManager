using System;
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

        public Room Create(Room room)
        {
            throw new NotImplementedException();
        }

        public Room Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(Room room)
        {
            throw new NotImplementedException();
        }

        public List<Room> FindAllRooms()
        {
            return roomDao.FindAll();
        }

        public Room GetRoom(int id)
        {
            throw new NotImplementedException();
        }
    }
}
