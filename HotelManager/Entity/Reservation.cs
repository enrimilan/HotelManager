using System;

namespace HotelManager.Entity
{
    public class Reservation
    {

        public int Id { get; set; }
        public DateTime From { get; set; }
        public string FromDateString { get; set; }
        public DateTime To { get; set; }
        public string ToDateString { get; set; }
        public string Person { get; set; }
        public string Contact { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationDateString { get; set; }
        public DateTime EndDate { get; set; }
        public string EndDateString { get; set; }
        public bool Canceled { get; set; }
        public bool Past { get; set; }
        public bool CheckedIn { get; set; }
        public string Status { get; set; }
        public Room Room { get; set; }
        public string RoomString { get; set; }
        
    }
}
