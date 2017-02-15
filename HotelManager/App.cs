using HotelManager.Entity;
using HotelManager.Service;
using System.Collections.Generic;

namespace HotelManager
{
    public partial class App
    {
        public static void Main()
        {
            RoomService roomService = ServiceFactory.GetRoomService();
            List<Room> rooms = roomService.FindAllRooms();
            foreach (Room room in rooms)
            {
                System.Diagnostics.Debug.WriteLine("Record: id=" + room.Id + " name=" + room.Number);
            }
        }

    }
}
