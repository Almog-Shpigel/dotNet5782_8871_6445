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
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationPage.xaml
    /// </summary>
    public partial class StationPage : Page
    {
        private ListViewItem item;
        private Frame MainFrame;
        private BlApi.IBL BLW;
        private BO.StationToList Station;
        private BO.StationBL StationBL;
        public StationPage(BlApi.IBL IBL, ListViewItem item,Frame frame)
        {
            InitializeComponent();
            BLW = IBL;
            this.item = item;
            MainFrame = frame;
            Station = (BO.StationToList)item.DataContext;
            StationBL = IBL.GetStation(Station.ID);
            DataContext = StationBL;
            DronesListView.ItemsSource = StationBL.ChargingDrones;
            AddNewStationPanell.Visibility = Visibility.Collapsed;
        }

        public StationPage(IBL bLW, RoutedEventArgs e, Frame mainFrame)
        {
            InitializeComponent();
            BLW = bLW;
            MainFrame = mainFrame;
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
                AddNewStationButton.IsEnabled = true;
            }
            else
            {
                AddNewStationButton.IsEnabled = false;
            }
        }

        private void UpdateNameButton_Click(object sender, RoutedEventArgs e)
        {
            BLW.UpdateStationName(Convert.ToInt32(IDBlock.Text), UpdateNameBlock.Text);
            DataContext = BLW.GetStation(Station.ID);
        }

        private void UpdateChargeSlotsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLW.UpdateStationSlots(Station.ID, Convert.ToInt32(UpdateChargeSlotsBlock.Text),StationBL.ChargingDrones.Count());
                DataContext = BLW.GetStation(Station.ID);
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
            try
            {
                BLW.DeleteStation(StationBL.ID);
                MainFrame.Content = new StationListPage(BLW, MainFrame);
            }
            catch (Exception exp)
            {

                InvalidInputBlock.Text = exp.Message;
                InvalidInputBlock.Visibility = Visibility.Visible;
            }
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                BO.DroneChargeBL drone = (BO.DroneChargeBL)item.DataContext;
                MainFrame.Content = new DronePage(BLW, MainFrame,drone.DroneID);
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
                AddNewStationButton.IsEnabled = false;
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


        private void AddNewStationButton_Click(object sender, RoutedEventArgs e)
        {
            StationBL NewStation = new(Convert.ToInt32(EnterStationIDBox.Text), EnterStationNameBox.Text, Convert.ToInt32(EnterAvailAbleSlotsBox.Text),
               new(Convert.ToDouble(EnterLattitudeBox.Text), Convert.ToDouble(EnterLongitudeBox.Text)));
            try
            {
                BLW.AddNewStation(NewStation);
                MainFrame.Content = new StationListPage(BLW, MainFrame);
            }
            catch (Exception exp)
            {

                InvalidInputBlock.Text = exp.Message;
                InvalidInputBlock.Visibility = Visibility.Visible;
                EnterStationIDBox.Foreground = Brushes.Red;
                AddNewStationButton.IsEnabled = false;
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
                AddNewStationButton.IsEnabled = false;
            }
            else
            {
                InvalidInputBlock.Visibility = Visibility.Collapsed;
                EnterStationIDBox.Foreground = Brushes.Black;
                EnableButton();
            }
        }
    }
}
