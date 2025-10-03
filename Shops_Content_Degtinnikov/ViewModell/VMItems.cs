using Shops_Content_Degtinnikov.Context;
using System.Collections.ObjectModel;
namespace Shops_Content_Degtinnikov.ViewModell
{
    public class VMItems
    {
        public ObservableCollection<ItemsContext> Items { get; set; }
    }
}
