using System.Data;
using System.Data.SQLite;
using System.Diagnostics;

namespace HotelManager.Dao.Impl
{
    public class SQLiteDao
    {

        protected SQLiteConnection connection;

        public SQLiteDao()
        {
            connection = DbConnection.GetConnection();
        }


        public DataTable ExecuteSql(SQLiteCommand command)
        {
            Trace.TraceInformation("Execute query {0}", command.CommandText);
            DataTable dt = new DataTable();
            SQLiteDataReader reader = command.ExecuteReader();
            dt.Load(reader);
            reader.Close();
            return dt;
        }

    }
}
