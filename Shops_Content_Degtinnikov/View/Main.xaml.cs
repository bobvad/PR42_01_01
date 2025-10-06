using System.Windows.Controls;

namespace Shops_Content_Degtinnikov.View
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public Main()
        {
            InitializeComponent();
            DataContext = new ViewModell.VMItems();
        }
    }
}
