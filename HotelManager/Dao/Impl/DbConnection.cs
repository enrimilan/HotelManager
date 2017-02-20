using System;
using System.Data.SQLite;

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
            currentPath = currentPath.Substring(0, currentPath.Length - 11);
            string absolutePath = System.IO.Path.Combine(currentPath, relativePath);
            string connectionString = string.Format("Data Source={0}", absolutePath);
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
