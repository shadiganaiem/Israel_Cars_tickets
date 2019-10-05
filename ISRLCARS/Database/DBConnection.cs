using System;
using MySql.Data.MySqlClient;
namespace ISRLCARS.Database
{
    public class DBConnection
    {
        MySqlConnection conn;
        string connectionString;
        public DBConnection()
        {
            connectionString = "server=162.241.218.181;port=3306;database=gfouapps_PeaceMeeting;uid=gfouapps_QRCheck;password=PeaceMeeting;charset=utf8";
            conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                Console.WriteLine("Connection Success!!");
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
            }
        }

        public MySqlConnection getConn()
        {
            return conn;
        }
    }
}
