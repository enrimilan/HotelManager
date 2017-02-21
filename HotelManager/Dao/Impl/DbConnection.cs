using System;
using System.Data.SQLite;
using System.Diagnostics;

namespace HotelManager.Dao
{
    public class DbConnection
    {
        private static SQLiteConnection connection = null;

        private DbConnection()
        {
            string relativePath = @"Database\hotel.db";
            string currentPath;
            currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string absolutePath = System.IO.Path.Combine(currentPath, relativePath);
            string connectionString = string.Format("Data Source={0}", absolutePath);
            Trace.TraceInformation("Connect to database using source '{0}'", absolutePath);
            connection = new SQLiteConnection(connectionString);
            connection.Open();
        }

        public static SQLiteConnection GetConnection()
        {
            if (connection == null)
            {
                new DbConnection();
            }
            return connection;
        }
    }
}
