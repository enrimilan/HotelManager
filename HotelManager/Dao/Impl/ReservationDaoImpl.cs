using System;
using System.Collections.Generic;
using HotelManager.Entity;
using System.Data;
using System.Globalization;

namespace HotelManager.Dao.Impl
{
    public class ReservationDaoImpl : SQLiteDao, ReservationDao
    {

        public void Save(Reservation reservation)
        {
            
            if (reservation.Id <= 0)
            {
                string from = reservation.From.ToString("yyyy.MM.dd HH:mm:ss");
                string to = reservation.To.ToString("yyyy.MM.dd HH:mm:ss");
                string created = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
                string query = string.Format("INSERT INTO reservations(roomid, room, fromDate, toDate, person, contact, created) VALUES ({0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", reservation.Room.Id, reservation.Room.Number, from, to, reservation.Person, reservation.Contact, created);
                System.Diagnostics.Debug.Write(query);
                ExecuteSql(query);
            }
            else
            {
                Update(reservation);
            }
        }

        private void Update(Reservation reservation)
        {
            string endDateString = reservation.EndDate.ToString("yyyy.MM.dd HH:mm:ss");
            string updateQuery = string.Format("UPDATE reservations SET status='{0}', canceled='{1}', past='{2}', checkedIn='{3}', endDate='{4}'", reservation.Status, reservation.Canceled, reservation.Past, reservation.CheckedIn, endDateString);
            updateQuery = updateQuery + " WHERE id = " + reservation.Id;
            ExecuteSql(updateQuery);
        }

        public List<Reservation> FindAll(Room room)
        {
            string query = string.Format("SELECT * FROM reservations WHERE canceled='False' AND past='False' AND roomid={0}", room.Id);
            DataTable dt = ExecuteSql(query);
            return ProcessReservationsResult(dt);
        }

        public List<Reservation> FindAll(string query)
        {
            string sqlQuery = string.Format("SELECT * FROM reservations WHERE canceled='False' AND past='False' AND (fromDate LIKE '%{0}%' OR toDate LIKE '%{0}%' OR status LIKE '%{0}%' OR room LIKE '%{0}%' OR person LIKE '%{0}%' OR contact LIKE '%{0}%') ORDER BY fromDate", query);
            DataTable dt = ExecuteSql(sqlQuery);
            return ProcessReservationsResult(dt);
        }

        public List<Reservation> FindAllCanceled(string query)
        {
            string sqlQuery = string.Format("SELECT * FROM reservations WHERE canceled='True' AND past='False' AND (fromDate LIKE '%{0}%' OR toDate LIKE '%{0}%' OR endDate LIKE '%{0}%' OR room LIKE '%{0}%' OR person LIKE '%{0}%' OR contact LIKE '%{0}%') ORDER BY fromDate", query);
            DataTable dt = ExecuteSql(sqlQuery);
            return ProcessReservationsResult(dt);
        }

        public List<Reservation> FindAllPast(string query)
        {
            string sqlQuery = string.Format("SELECT * FROM reservations WHERE canceled='False' AND past='True' AND (fromDate LIKE '%{0}%' OR toDate LIKE '%{0}%' OR endDate LIKE '%{0}%' OR room LIKE '%{0}%' OR person LIKE '%{0}%' OR contact LIKE '%{0}%') ORDER BY fromDate", query);
            DataTable dt = ExecuteSql(sqlQuery);
            return ProcessReservationsResult(dt);
        }

        private List<Reservation> ProcessReservationsResult(DataTable dt)
        {
            List<Reservation> reservations = new List<Reservation>();
            foreach (DataRow row in dt.Rows)
            {
                Reservation reservation = new Reservation();
                reservation.Id = int.Parse(row["id"].ToString());
                reservation.From = DateTime.ParseExact((string)row["fromDate"], "yyyy.MM.dd HH:mm:ss", CultureInfo.CurrentCulture);
                reservation.FromDateString = reservation.From.ToString("yyyy.MM.dd");
                reservation.To = DateTime.ParseExact((string)row["toDate"], "yyyy.MM.dd HH:mm:ss", CultureInfo.CurrentCulture);
                reservation.ToDateString = reservation.To.ToString("yyyy.MM.dd");
                reservation.CreationDate = DateTime.ParseExact((string)row["created"], "yyyy.MM.dd HH:mm:ss", CultureInfo.CurrentCulture);
                reservation.CreationDateString = reservation.CreationDate.ToString("yyyy.MM.dd HH:mm:ss");
                reservation.Person = (string)row["person"];
                reservation.Contact = (string)row["contact"];
                reservation.Status = (string)row["status"];
                reservation.Canceled = bool.Parse((string)row["canceled"]);
                reservation.Past = bool.Parse((string)row["past"]);
                reservation.CheckedIn = bool.Parse((string)row["checkedIn"]);
                reservation.RoomString = (string)row["room"];
                if (!(row["endDate"] is System.DBNull))
                {
                    reservation.EndDate = DateTime.ParseExact((string)row["endDate"], "yyyy.MM.dd HH:mm:ss", CultureInfo.CurrentCulture);
                    reservation.EndDateString = reservation.EndDate.ToString("yyyy.MM.dd HH:mm:ss");
                }
                reservations.Add(reservation);
            }

            return reservations;
        }

    }
}
