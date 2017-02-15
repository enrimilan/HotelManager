using System.Collections.Generic;
using HotelManager.Entity;
using System.Data;

namespace HotelManager.Dao.Impl
{
    public class RoomDaoImpl : SQLiteDao, RoomDao
    {

        public List<Room> FindAll()
        {
            List<Room> rooms = new List<Room>();
            DataTable dt = ExecuteSql("SELECT * FROM rooms");
            foreach (DataRow row in dt.Rows)
            {
                Room room = new Room((string)row["number"]);
                room.Id = int.Parse(row["id"].ToString());
                rooms.Add(room);
            }

            return rooms;
        }
    }
}
