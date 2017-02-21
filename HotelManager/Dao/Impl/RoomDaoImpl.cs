using System.Collections.Generic;
using HotelManager.Entity;
using System.Data;
using System;
using System.Globalization;
using System.Data.SQLite;
using HotelManager.Util;

namespace HotelManager.Dao.Impl
{
    public class RoomDaoImpl : SQLiteDao, RoomDao
    {

        private const string RoomsTable = "rooms";
        private const string IdCol = "id";
        private const string NumberCol = "number";
        private const string CreatedCol = "created";
        private const string OldCol = "old";
        private const string ReservationsCol = "reservations";
        private const string StatusCol = "status";
        private const string MovedCol = "moved";

        public void Save(Room room)
        {

            if (room.Id <= 0)
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = $"INSERT INTO {RoomsTable} ({NumberCol}, {CreatedCol}) VALUES (@numberVal, @createdVal);";
                
                command.Parameters.Add(new SQLiteParameter("@numberVal", room.Number));
                command.Parameters.Add(new SQLiteParameter("@createdVal", room.CreationDateString));
                ExecuteSql(command);
            }
            else
            {
                Update(room);
            }
            
        }

        private void Update(Room room)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = $"UPDATE {RoomsTable} SET {OldCol}=@oldVal, {ReservationsCol}=@reservationsVal, {StatusCol}=@statusVal, {MovedCol}=@movedVal WHERE {IdCol}=@IdVal;";

            command.Parameters.Add(new SQLiteParameter("@oldVal", room.IsOld+""));
            command.Parameters.Add(new SQLiteParameter("@reservationsVal", room.Reservations));
            command.Parameters.Add(new SQLiteParameter("@statusVal", room.Status));
            command.Parameters.Add(new SQLiteParameter("@movedVal", room.MovedDateString));
            command.Parameters.Add(new SQLiteParameter("@idVal", room.Id));

            ExecuteSql(command);
        }

        public List<Room> Find(string query)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = $"SELECT * FROM {RoomsTable} WHERE {OldCol}='False' AND ({NumberCol} LIKE @query OR status LIKE @query);";
            command.Parameters.Add(new SQLiteParameter("@query", "%"+query+"%"));
            DataTable dt = ExecuteSql(command);
            return ProcessRoomsResult(dt);
        }

        public Room Find(int id)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = $"SELECT * FROM {RoomsTable} WHERE {IdCol}=@IdVal;";
            command.Parameters.Add(new SQLiteParameter("@idVal", id));
            DataTable dt = ExecuteSql(command);
            return ProcessRoomsResult(dt)[0];
        }

        public List<Room> FindOld(string query)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = $"SELECT * FROM {RoomsTable} WHERE {OldCol}='True' AND ({NumberCol} LIKE @query OR status LIKE @query);";
            command.Parameters.Add(new SQLiteParameter("@query", "%" + query + "%"));
            DataTable dt = ExecuteSql(command);
            return ProcessRoomsResult(dt);
        }

        private List<Room> ProcessRoomsResult(DataTable dt)
        {
            List<Room> rooms = new List<Room>();
            foreach (DataRow row in dt.Rows)
            {
                Room room = new Room((string)row[NumberCol]);
                room.Id = int.Parse(row[IdCol].ToString());
                room.Status = (string)row[StatusCol];
                room.Reservations = int.Parse(row[ReservationsCol].ToString());
                room.CreationDate = DateTime.ParseExact((string) row[CreatedCol], Constants.DateFormat, CultureInfo.CurrentCulture);
                room.CreationDateString = room.CreationDate.ToString(Constants.DateFormat);
                if(!(row[MovedCol] is  System.DBNull))
                {
                    room.MovedDateString = (string)row[MovedCol];
                }
                
                rooms.Add(room);
            }

            return rooms;
        }

    }
}
