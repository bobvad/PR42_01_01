using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shops_Content_Degtinnikov.Classes
{
    public class Connection
    {
          public static readonly string config = "server=127.0.0.1;uid=root;pwd=;database=ShopContent";
          public static MySqlConnection OpenConnection()
          {
               MySqlConnection connection = new MySqlConnection(config);
               connection.Open();
               return connection;
          }
          public static MySqlDataReader Query(string SQL, MySqlConnection connection)
          {
              return new MySqlCommand(SQL, connection).ExecuteReader();
          }
          public static void CloseConnection(MySqlConnection connection)
          {
              connection.Close();
              MySqlConnection.ClearPool(connection);
          }
    }
}
