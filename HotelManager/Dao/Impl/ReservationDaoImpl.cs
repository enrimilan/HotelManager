using System;
using System.Collections.Generic;
using HotelManager.Entity;
using System.Data;
using System.Globalization;
using System.Data.SQLite;
using HotelManager.Util;

namespace HotelManager.Dao.Impl
{
    public class ReservationDaoImpl : SQLiteDao, ReservationDao
    {

        private const string ReservationsTable = "reservations";
        private const string IdCol = "id";
        private const string RoomidCol = "roomid";
        private const string RoomCol = "room";
        private const string FromDateCol = "fromDate";
        private const string ToDateCol = "toDate";
        private const string PersonCol = "person";
        private const string ContactCol = "contact";
        private const string CreatedCol = "created";
        private const string StatusCol = "status";
        private const string CanceledCol = "canceled";
        private const string CheckedInCol = "checkedIn";
        private const string PastCol = "past";
        private const string EndDateCol = "endDate";

        public void Save(Reservation reservation)
        {
            
            if (reservation.Id <= 0)
            {

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = $"INSERT INTO {ReservationsTable} ({RoomidCol}, {RoomCol}, {FromDateCol}, {ToDateCol}, {PersonCol}, {ContactCol}, {CreatedCol}) VALUES (@roomidVal, @roomVal, @fromDateVal, @toDateVal, @personVal, @contactVal, @createdVal);";
                command.Parameters.Add(new SQLiteParameter("@roomidVal", reservation.Room.Id));
                command.Parameters.Add(new SQLiteParameter("@roomVal", reservation.Room.Number));
                command.Parameters.Add(new SQLiteParameter("@fromDateVal", reservation.FromDateString));
                command.Parameters.Add(new SQLiteParameter("@toDateVal", reservation.ToDateString));
                command.Parameters.Add(new SQLiteParameter("@personVal", reservation.Person));
                command.Parameters.Add(new SQLiteParameter("@contactVal", reservation.Contact));
                command.Parameters.Add(new SQLiteParameter("@createdVal", reservation.CreationDateString));

                ExecuteSql(command);
            }
            else
            {
                Update(reservation);
            }
        }

        private void Update(Reservation reservation)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = $"UPDATE {ReservationsTable} SET {StatusCol}=@statusVal, {CanceledCol}=@canceledVal, {PastCol}=@pastVal, {CheckedInCol}=@checkedInVal, {EndDateCol}=@endDateVal WHERE {IdCol}=@idVal;";
            command.Parameters.Add(new SQLiteParameter("@statusVal", reservation.Status));
            command.Parameters.Add(new SQLiteParameter("@canceledVal", reservation.Canceled+""));
            command.Parameters.Add(new SQLiteParameter("@pastVal", reservation.Past+""));
            command.Parameters.Add(new SQLiteParameter("@checkedInVal", reservation.CheckedIn+""));
            command.Parameters.Add(new SQLiteParameter("@endDateVal", reservation.EndDateString));
            command.Parameters.Add(new SQLiteParameter("@idVal", reservation.Id));
            
            ExecuteSql(command);
        }

        public List<Reservation> FindAll(Room room)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = $"SELECT * FROM {ReservationsTable} WHERE {CanceledCol}='False' AND {PastCol}='False' AND {RoomidCol}=@roomidVal ORDER BY {FromDateCol};";
            command.Parameters.Add(new SQLiteParameter("@roomidVal", room.Id));
            DataTable dt = ExecuteSql(command);
            return ProcessReservationsResult(dt);
        }

        public List<Reservation> FindAll(string query)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = $"SELECT * FROM {ReservationsTable} WHERE {CanceledCol}='False' AND {PastCol}='False' AND ({FromDateCol} LIKE @query OR {ToDateCol} LIKE @query OR {StatusCol} LIKE @query OR {RoomCol} LIKE @query OR {PersonCol} LIKE @query OR {ContactCol} LIKE @query) ORDER BY {FromDateCol};";
            command.Parameters.Add(new SQLiteParameter("@query", "%" + query + "%"));
            DataTable dt = ExecuteSql(command);
            return ProcessReservationsResult(dt);
        }

        public List<Reservation> FindAllCanceled(string query)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = $"SELECT * FROM {ReservationsTable} WHERE {CanceledCol}='True' AND {PastCol}='False' AND ({FromDateCol} LIKE @query OR {ToDateCol} LIKE @query OR {EndDateCol} LIKE @query OR {RoomCol} LIKE @query OR {PersonCol} LIKE @query OR {ContactCol} LIKE @query) ORDER BY {FromDateCol};";
            command.Parameters.Add(new SQLiteParameter("@query", "%" + query + "%"));
            DataTable dt = ExecuteSql(command);
            return ProcessReservationsResult(dt);
        }

        public List<Reservation> FindAllPast(string query)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = $"SELECT * FROM {ReservationsTable} WHERE {CanceledCol}='False' AND {PastCol}='True' AND ({FromDateCol} LIKE @query OR {ToDateCol} LIKE @query OR {EndDateCol} LIKE @query OR {RoomCol} LIKE @query OR {PersonCol} LIKE @query OR {ContactCol} LIKE @query) ORDER BY {FromDateCol};";
            command.Parameters.Add(new SQLiteParameter("@query", "%" + query + "%"));
            DataTable dt = ExecuteSql(command);
            return ProcessReservationsResult(dt);
        }

        private List<Reservation> ProcessReservationsResult(DataTable dt)
        {
            List<Reservation> reservations = new List<Reservation>();
            foreach (DataRow row in dt.Rows)
            {
                Reservation reservation = new Reservation();
                reservation.Id = int.Parse(row["id"].ToString());
                reservation.From = DateTime.ParseExact((string)row[FromDateCol], Constants.DateFormat, CultureInfo.CurrentCulture);
                reservation.FromDateString = reservation.From.ToString(Constants.DateFormatWithoutTime);
                reservation.To = DateTime.ParseExact((string)row[ToDateCol], Constants.DateFormat, CultureInfo.CurrentCulture);
                reservation.ToDateString = reservation.To.ToString(Constants.DateFormatWithoutTime);
                reservation.CreationDate = DateTime.ParseExact((string)row[CreatedCol], Constants.DateFormat, CultureInfo.CurrentCulture);
                reservation.CreationDateString = reservation.CreationDate.ToString(Constants.DateFormat);
                reservation.Person = (string)row[PersonCol];
                reservation.Contact = (string)row[ContactCol];
                reservation.Status = (string)row[StatusCol];
                reservation.Canceled = bool.Parse((string)row[CanceledCol]);
                reservation.Past = bool.Parse((string)row[PastCol]);
                reservation.CheckedIn = bool.Parse((string)row[CheckedInCol]);
                reservation.RoomString = (string)row[RoomCol];
                Room room = new Room(reservation.RoomString);
                room.Id = int.Parse(row[RoomidCol].ToString());
                reservation.Room = room;
                if (!(row[EndDateCol] is System.DBNull))
                {
                    reservation.EndDate = DateTime.ParseExact((string)row[EndDateCol], Constants.DateFormat, CultureInfo.CurrentCulture);
                    reservation.EndDateString = reservation.EndDate.ToString(Constants.DateFormat);
                }
                reservations.Add(reservation);
            }

            return reservations;
        }

    }
}
