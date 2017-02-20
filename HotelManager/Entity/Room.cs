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
        public string CreationDateString { get; set; }
        public bool IsOld { get; set; }
        public string MovedDateString { get; set; }

        public Room(string number)
        {
            Number = number;
        }

    }
}
