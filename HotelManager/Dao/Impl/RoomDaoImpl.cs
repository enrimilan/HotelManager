using System.Collections.Generic;
using HotelManager.Entity;
using System.Data;
using System;
using System.Globalization;

namespace HotelManager.Dao.Impl
{
    public class RoomDaoImpl : SQLiteDao, RoomDao
    {

        public void Save(Room room)
        {
            if (room.Id <= 0)
            {
                ExecuteSql("INSERT INTO rooms(NUMBER, CREATED) VALUES ('" + room.Number + "', '"+ room.CreationDate.ToString("yyyy.MM.dd HH:mm:ss") +"')");
            }
            else
            {
                Update(room);
            }
            
        }

        private void Update(Room room)
        {
            string updateQuery = "UPDATE rooms SET old='" + room.IsOld + "', reservations='" + room.Reservations + "'";
   
            if(room.Status != null)
            {
                updateQuery = updateQuery + ", status='"+room.Status+"'";
            }
            if(room.MovedDateString != null)
            {
                updateQuery = updateQuery + ", moved='" + room.MovedDateString + "'";
            }
            
            updateQuery = updateQuery + " WHERE id = " + room.Id;
            ExecuteSql(updateQuery);
        }

        public List<Room> Find(string query)
        {
            DataTable dt = ExecuteSql("SELECT * FROM rooms WHERE old='False' AND (number LIKE '%" + query + "%' OR status LIKE '%" + query + "%')");
            return ProcessRoomsResult(dt);
        }

        public Room Find(int id)
        {
            DataTable dt = ExecuteSql("SELECT * FROM rooms WHERE id=" + id);
            return ProcessRoomsResult(dt)[0];
        }

        public List<Room> FindOld(string query)
        {
            DataTable dt = ExecuteSql("SELECT * FROM rooms WHERE old='True' AND (number LIKE '%" + query + "%' OR moved LIKE '%" + query +"%')");
            return ProcessRoomsResult(dt);
        }

        private List<Room> ProcessRoomsResult(DataTable dt)
        {
            List<Room> rooms = new List<Room>();
            foreach (DataRow row in dt.Rows)
            {
                Room room = new Room((string)row["number"]);
                room.Id = int.Parse(row["id"].ToString());
                room.Status = (string)row["status"];
                room.Reservations = int.Parse(row["reservations"].ToString());
                room.CreationDate = DateTime.ParseExact((string) row["created"], "yyyy.MM.dd HH:mm:ss", CultureInfo.CurrentCulture);
                room.CreationDateString = room.CreationDate.ToString("yyyy.MM.dd HH:mm:ss");
                if(!(row["moved"] is  System.DBNull))
                {
                    room.MovedDateString = (string)row["moved"];
                }
                
                rooms.Add(room);
            }

            return rooms;
        }

    }
}
