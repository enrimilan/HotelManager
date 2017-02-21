using System.Diagnostics;

namespace HotelManager.Dao.Impl
{
    public class MainDaoImpl : MainDao
    {
        public void Close()
        {
            Trace.TraceInformation("Close connection to sqlite database.");
            DbConnection.GetConnection().Close();
        }
    }
}
