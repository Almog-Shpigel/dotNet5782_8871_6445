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
        private Frame MainFrame;
        public StationListPage(Frame Main)
        {
            InitializeComponent();
            BlApi.IBL BLW = BlFactory.GetBl();
            MainFrame = Main;
            StationListView.ItemsSource = BLW.GetAllStations();
        }

        private void AddNewStationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackWindow_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();// TO DO: to find a way to go back to main window without openning it again
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
