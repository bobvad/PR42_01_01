using Shops_Content_Degtinnikov.Context;
using Shops_Content_Degtinnikov.ViewModell;
using System.Windows.Controls;

namespace Shops_Content_Degtinnikov.View
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Page
    {
        public Add(ItemsContext item)
        {
            InitializeComponent();
            this.DataContext = item;
            this.cbCategory.ItemsSource = new VMCategorys().Categorys;
            this.cbCategory.DisplayMemberPath = "Name"; 
            this.cbCategory.SelectedValuePath = "Id";  
            if (item.Category != null)
            {
                this.cbCategory.SelectedValue = item.Category.Id;
            }
        }
    }
}