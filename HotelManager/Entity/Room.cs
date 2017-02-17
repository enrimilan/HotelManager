using System;

namespace HotelManager.Entity
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public int Reservations { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime NextReservation { get; set; }

        public Room(string number)
        {
            Number = number;
            Status = "Free";
            Reservations = 0;
            CreationDate = DateTime.Now;
            NextReservation = DateTime.Now;
        }
    }
}
