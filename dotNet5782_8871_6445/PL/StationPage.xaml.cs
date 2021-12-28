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
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationPage.xaml
    /// </summary>
    public partial class StationPage : Page
    {
        private IBL IBL = BlFactory.GetBl();
        private StationToList Station;
        private StationBL StationBL;
        public StationPage(StationToList item)
        {
            InitializeComponent();
            Station = item;
            StationBL = IBL.GetStation(Station.ID);
            DataContext = StationBL;
            DronesListViewFromStation.ItemsSource = StationBL.ChargingDrones;
            AddNewStationPanell.Visibility = Visibility.Collapsed;
        }

        public StationPage(RoutedEventArgs e)
        {
            InitializeComponent();
            ShowDetailsStation.Visibility = Visibility.Collapsed;
            UpdateBlocksPanel.Visibility = Visibility.Collapsed;
            UpdateNameBlock.Visibility = Visibility.Collapsed;
            UpdateChargeSlotsBlock.Visibility = Visibility.Collapsed;
            InvalidInputBlock.Visibility = Visibility.Collapsed;
            PrintLocationBlock.Text = "Lattitude:";
            PrintDronsChargingBlock.Text = "Longitude";
            EnableButton();
           
        }
        private void EnableButton()
        {

            if (EnterStationIDBox.Text != "" && EnterStationNameBox.Text != ""
                && EnterAvailAbleSlotsBox.Text != "" && EnterLattitudeBox.Text != "" &&
                EnterLongitudeBox.Text != "" && InvalidInputBlock.Visibility != Visibility.Visible)
            {
                StationEntityAddButton.IsEnabled = true;
            }
            else
            {
                StationEntityAddButton.IsEnabled = false;
            }
        }

        private void UpdateNameButton_Click(object sender, RoutedEventArgs e)
        {
            IBL.UpdateStationName(Convert.ToInt32(IDBlock.Text), UpdateNameBlock.Text);
            DataContext = IBL.GetStation(Station.ID);
        }

        private void UpdateChargeSlotsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IBL.UpdateStationSlots(Station.ID, Convert.ToInt32(UpdateChargeSlotsBlock.Text),StationBL.ChargingDrones.Count());
                DataContext = IBL.GetStation(Station.ID);
            }
            catch (Exception exp)
            {
                InvalidInputBlock.Text = exp.Message;
                InvalidInputBlock.Visibility = Visibility.Visible;
            }
            
        }
        private void EnterChargeSlotsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int Slots;
            if (!int.TryParse(UpdateChargeSlotsBlock.Text, out Slots))
            {
                InvalidInputBlock.Visibility = Visibility.Visible;
                UpdateChargeSlotsBlock.Foreground = Brushes.Red;
                UpdateChargeSlotsButton.IsEnabled = false;
            }
            else
            {
                UpdateChargeSlotsButton.IsEnabled = true;
                InvalidInputBlock.Visibility = Visibility.Collapsed;
                UpdateChargeSlotsBlock.Foreground = Brushes.Black;
            }
        }

        private void UpdateDeleteStationButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Are you sure you want to delete selected station?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res == MessageBoxResult.No)
                return;
            MessageBox.Show("This method is under construction!", "TBD", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
            MessageBox.Show("This method is under construction!", "TBD", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            MessageBox.Show("This method is under construction!", "TBD", MessageBoxButton.YesNo, MessageBoxImage.Stop);
            MessageBox.Show("This method is under construction!", "TBD", MessageBoxButton.YesNoCancel, MessageBoxImage.Hand);
            try
            {
                IBL.DeleteStation(StationBL.ID);
            }
            catch (Exception exp)
            {
                InvalidInputBlock.Text = exp.Message;
                InvalidInputBlock.Visibility = Visibility.Visible;
            }
        }

        private void EnterStationIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int stationID;
            if (!int.TryParse(EnterStationIDBox.Text, out stationID))
            {
                InvalidInputBlock.Visibility = Visibility.Visible;
                InvalidInputBlock.Text ="Station id must contain only numbers";
                EnterStationIDBox.Foreground = Brushes.Red;
                StationEntityAddButton.IsEnabled = false;
            }
            else
            {
                InvalidInputBlock.Visibility = Visibility.Collapsed;
                EnterStationIDBox.Foreground = Brushes.Black;
                EnableButton();
            }
        }

        private void EnterStationNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }


        private void StationEntityAddButton_Click(object sender, RoutedEventArgs e)
        {
            StationBL NewStation = new(Convert.ToInt32(EnterStationIDBox.Text), EnterStationNameBox.Text, Convert.ToInt32(EnterAvailAbleSlotsBox.Text),
               new(Convert.ToDouble(EnterLattitudeBox.Text), Convert.ToDouble(EnterLongitudeBox.Text)));
            try
            {
                IBL.AddNewStation(NewStation);
                NavigationService.GoBack();
            }
            catch (Exception exp)
            {
                InvalidInputBlock.Text = exp.Message;
                InvalidInputBlock.Visibility = Visibility.Visible;
                EnterStationIDBox.Foreground = Brushes.Red;
                StationEntityAddButton.IsEnabled = false; 
            }
        }

        private void EnterAvailAbleSlotsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int stationID;
            if (!int.TryParse(EnterAvailAbleSlotsBox.Text, out stationID))
            {
                InvalidInputBlock.Visibility = Visibility.Visible;
                InvalidInputBlock.Text = "Available slots must be a number";
                EnterAvailAbleSlotsBox.Foreground = Brushes.Red;
                StationEntityAddButton.IsEnabled = false;
            }
            else
            {
                InvalidInputBlock.Visibility = Visibility.Collapsed;
                EnterStationIDBox.Foreground = Brushes.Black;
                EnableButton();
            }
        }

        private void StationListGoBackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
