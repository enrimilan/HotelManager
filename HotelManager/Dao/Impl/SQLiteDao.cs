using System.Data;
using System.Data.SQLite;

namespace HotelManager.Dao.Impl
{
    public class SQLiteDao
    {
        protected SQLiteConnection connection;

        public SQLiteDao()
        {
            connection = DbConnection.GetConnection();
        }

        public DataTable ExecuteSql(string query)
        {
            SQLiteCommand cmd = new SQLiteCommand(connection);
            cmd.CommandText = query + ";";
            DataTable dt = new DataTable();
            SQLiteDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            reader.Close();
            return dt;
        }

    }
}
