using MySql.Data.MySqlClient;
using Shops_Content_Degtinnikov.Classes;
using Shops_Content_Degtinnikov.Modell;
using System.Collections.ObjectModel;
using System.Globalization;

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
            Category = new Category();
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

            try
            {
                if (isNew)
                {
                    string priceValue = this.Price.ToString(CultureInfo.InvariantCulture).Replace(',', '.');
                    string insertQuery = $"INSERT INTO Items (Name, Price, Description) " +
                                       $"VALUES ('{this.Name}', {priceValue}, '{this.Description}')";

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    string selectIdQuery = "SELECT LAST_INSERT_ID()";
                    using (MySqlCommand command = new MySqlCommand(selectIdQuery, connection))
                    using (MySqlDataReader dataId = command.ExecuteReader())
                    {
                        if (dataId.Read())
                        {
                            this.Id = dataId.GetInt32(0);
                        }
                    }
                }
                else
                {
                    string categoryId = this.Category?.Id.ToString() ?? "NULL";

                    string priceValue = this.Price.ToString(CultureInfo.InvariantCulture).Replace(',', '.');
                    string updateQuery = $"UPDATE Items " +
                                       $"SET Name = '{this.Name}', " +
                                       $"Price = {priceValue}, " +
                                       $"Description = '{this.Description}', " +
                                       $"IdCategory = {categoryId} " +
                                       $"WHERE Id = {this.Id}";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            finally
            {
                Connection.CloseConnection(connection);
            }
        }
        public void Delete()
        {
            MySqlConnection connection = Connection.OpenConnection();
            Connection.Query($"DELETE FROM Items WHERE Id = {this.Id}", connection);
            Connection.CloseConnection(connection);
        }
        public RelayCommand OnDelete
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    Delete();
                    (MainWindow.init.Main.DataContext as ViewModell.VMItems).Items.Remove(this);
                });
            }
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
                    Save();
                    var viewModel = MainWindow.init.Main.DataContext as ViewModell.VMItems;
                    if (viewModel != null)
                    {
                        int index = viewModel.Items.IndexOf(this);
                        if (index != -1)
                        {
                            viewModel.Items[index] = this;
                        }
                    }

                    MainWindow.init.frame.Navigate(new View.Main());
                });
            }
        }
    }
}