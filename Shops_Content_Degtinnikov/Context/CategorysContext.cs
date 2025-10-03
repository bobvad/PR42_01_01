using MySql.Data.MySqlClient;
using Shops_Content_Degtinnikov.Classes;
using Shops_Content_Degtinnikov.Modell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shops_Content_Degtinnikov.Context
{
    public class CategorysContext: Category
    {
        public static ObservableCollection<Category> AllCategorys()
        {
            ObservableCollection<Category> allCategorys = new ObservableCollection<Category>();
            MySqlConnection connection = Connection.OpenConnection();
            MySqlDataReader dataReader = Connection.Query("SELECT * FROM Categorys", connection);

            while (dataReader.Read())
            {
                allCategorys.Add(new CategorysContext()
                {
                    Id = dataReader.GetInt32(0),
                    Name = dataReader.GetString(1)
                });
            }

            Connection.CloseConnection(connection);
            return allCategorys;
        }
    }
}
