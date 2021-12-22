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
        private BlApi.IBL BLW;
        private Frame MainFrame;
        public StationListPage(BlApi.IBL IBL, Frame Main)
        {
            InitializeComponent();
            BLW = IBL;
            MainFrame = Main;
            StationListView.ItemsSource = BLW.GetAllStations();
            UpdateLayout();
        }

        private void AddNewStationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackWindow_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();// TO DO: to find a way to go back to main window without openning it again
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                MainFrame.Content = new StationPage(BLW, item);
            }
        }
    

        private void StationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
