using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListPage.xaml
    /// </summary>
    public partial class StationListPage : Page
    {
        private BlApi.IBL IBL = BlFactory.GetBl();
        public StationListPage()
        {
            InitializeComponent();
            StationListView.ItemsSource = this.IBL.GetAllStations();
            UpdateLayout();
        }

        private void StationListAddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackWindow_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                //MainFrame.Content = new StationPage(item);
            }
        }
    

        private void StationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
