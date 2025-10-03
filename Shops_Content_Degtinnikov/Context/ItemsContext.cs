using MySql.Data.MySqlClient;
using Shops_Content_Degtinnikov.Classes;
using Shops_Content_Degtinnikov.Modell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Shops_Content_Degtinnikov.Context
{
    public class ItemsContext : Items
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }

        public ItemsContext(bool save = false)
        {
            if (save)
                Save(true);
        }
        public ItemsContext(Category category)
        {
            Category = category;
        }

        public static ObservableCollection<ItemsContext> AllItems()
        {
            ObservableCollection<ItemsContext> allItems = new ObservableCollection<ItemsContext>();
            ObservableCollection<CategorysContext> allCategories = CategorysContext.AllCategorys();

            MySqlConnection connection = Connection.OpenConnection();
            MySqlDataReader dataReader = Connection.Query("SELECT * FROM Items", connection);

            while (dataReader.Read())
            {
                allItems.Add(new ItemsContext()
                {
                    Id = dataReader.GetInt32(0),
                    Name = dataReader.GetString(1),
                    Price = dataReader.GetDecimal(2),
                    Description = dataReader.GetString(3),
                    Category = dataReader.IsDBNull(4) ? null : allCategories.FirstOrDefault(x => x.Id == dataReader.GetInt32(4))
                });
            }

            Connection.CloseConnection(connection);
            return allItems;
        }
        public void Save(bool isNew = false)
        {
            MySqlConnection connection = Connection.OpenConnection();

            if (isNew)
            {
                MySqlDataReader dataItem = Connection.Query(
                    $"INSERT INTO Items (" +
                    $"Name, " +
                    $"Price, " +
                    $"Description) " +
                    $"OUTPUT Inserted.Id " +
                    $"VALUES (" +
                    $"N'{this.Name}', " +
                    $"{this.Price}, " +
                    $"N'{this.Description}')", connection);

                dataItem.Read();
                this.Id = dataItem.GetInt32(0);
            }
            else
            {
                string categoryId = this.Category?.Id.ToString() ?? "NULL";

                Connection.Query(
                    $"UPDATE Items " +
                    $"SET " +
                    $"Name = N'{this.Name}', " +
                    $"Price = {this.Price}, " +
                    $"Description = N'{this.Description}', " +
                    $"IdCategory = {categoryId} " +
                    $"WHERE " +
                    $"Id = {this.Id}", connection);
            }

            Connection.CloseConnection(connection);
        }

        public void Delete()
        {
            MySqlConnection connection = Connection.OpenConnection();
            Connection.Query($"DELETE FROM Items WHERE Id = {this.Id}", connection);
            Connection.CloseConnection(connection);
        }
        public RelayCommand OnEdit
        {
            get 
            {
                return new RelayCommand(obj => 
                {
                    MainWindow.init.frame.Navigate(new View.Add(this));
                });
            }
        }
        public RelayCommand OnSave
        {
            get 
            {
                return new RelayCommand(obj => 
                {
                    Category = CategorysContext.AllCategorys().Where(x => x.Id == this.Category.Id).First();
                    Save();
                });
            }
        }
    }
}
