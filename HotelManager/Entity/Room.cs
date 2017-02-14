namespace HotelManager.Entity
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; }
        
        public Room(string number)
        {
            Number = number;
        }
    }
}
