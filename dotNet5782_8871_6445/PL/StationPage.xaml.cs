using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BL;
using BlApi;
using BO;
using Microsoft.Maps.MapControl.WPF;

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
            StationEntityAddButton.Visibility = Visibility.Collapsed;
            IconEntityAdded.Visibility = Visibility.Collapsed;
            Station = item;
            StationBL = IBL.GetStation(Station.ID);
            DataContext = StationBL;
            DronesListViewFromStation.ItemsSource = StationBL.ChargingDrones;
            Pushpin pin = new();
            ToolTip tt = new();
            pin.Location = new(StationBL.Location.Latitude, StationBL.Location.Longitude);
            pin.Tag = StationBL.ID;
            tt.Content = StationBL.ToString();
            pin.ToolTip = tt;
            //myMap.Children.Add(pin);
            //myMap.Center = pin.Location;
        }

        public StationPage(RoutedEventArgs e)
        {
            InitializeComponent();
            IDBox.IsReadOnly = false;
            AvailableSlotsBox.IsReadOnly = false;
            UpdateNameButton.Visibility = Visibility.Collapsed;
            UpdateTotalSlotsBox.Visibility = Visibility.Collapsed;
            IconEntityAdded.Visibility = Visibility.Collapsed;
            EnableButton();
           
        }
        private void EnableButton()
        {

            if (IDBox.Text != "" && NamelBlock.Text != ""
                && AvailableSlotsBox.Text != "" && EnterLatitudeBox.Text != "" &&
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

            IBL.UpdateStationName(Convert.ToInt32(IDBox.Text), NamelBlock.Text);
            DataContext = IBL.GetStation(Station.ID);
        }

        private void UpdateChargeSlotsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IBL.UpdateStationSlots(Station.ID, Convert.ToInt32(UpdateTotalSlotsBox.Text),StationBL.ChargingDrones.Count());
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
            if (int.TryParse(AvailableSlotsBox.Text, out _))
            {
                UpdateChargeSlotsButton.IsEnabled = true;
                InvalidInputBlock.Visibility = Visibility.Collapsed;
                AvailableSlotsBox.Foreground = Brushes.Black;
                EnableButton();
            }
            else
            {
                InvalidInputBlock.Visibility = Visibility.Visible;
                AvailableSlotsBox.Foreground = Brushes.Red;
                UpdateChargeSlotsButton.IsEnabled = false;
            }
        }

        private void EnterStationIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(IDBox.Text, out _))
            {
                InvalidInputBlock.Visibility = Visibility.Collapsed;
                IDBox.Foreground = Brushes.Black;
                EnableButton();
            }
            else
            {
                InvalidInputBlock.Visibility = Visibility.Visible;
                InvalidInputBlock.Text = "Station id must contain only numbers";
                IDBox.Foreground = Brushes.Red;
                StationEntityAddButton.IsEnabled = false;
            }
        }

        private void EnterStationNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }


        private void StationEntityAddButton_Click(object sender, RoutedEventArgs e)
        {
            StationBL NewStation = new(Convert.ToInt32(IDBox.Text), NamelBlock.Text, Convert.ToInt32(AvailableSlotsBox.Text),
               new(Convert.ToDouble(EnterLatitudeBox.Text), Convert.ToDouble(EnterLongitudeBox.Text)));
            try
            {
                IBL.AddNewStation(NewStation);
                StationEntityAddButton.IsEnabled = false;
                InvalidInputBlock.Text = "Station added!";
                InvalidInputBlock.Visibility = Visibility.Visible;
                InvalidInputBlock.Foreground = Brushes.Green;
                IDBox.IsEnabled = false;
                NamelBlock.IsEnabled = false;
                AvailableSlotsBox.IsEnabled = false;
                EnterLongitudeBox.IsEnabled = false;
                EnterLatitudeBox.IsEnabled = false;
                IconEntityAdded.Visibility = Visibility.Visible;
            }
            catch (Exception exp)
            {
                InvalidInputBlock.Text = exp.Message;
                InvalidInputBlock.Visibility = Visibility.Visible;
            }
        }

        private void StationListGoBackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EnableButton();
        }
        private void EnterLatitudeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int latitude;
            if (int.TryParse(EnterLatitudeBox.Text, out latitude))
            {
                InvalidInputBlock.Visibility = Visibility.Collapsed;
                UpdateTotalSlotsBox.Foreground = Brushes.Black;
                EnableButton();
            }
            else
            {
                InvalidInputBlock.Visibility = Visibility.Visible;
                InvalidInputBlock.Text = "Latitude must be a number";
                EnterLatitudeBox.Foreground = Brushes.Red;
                StationEntityAddButton.IsEnabled = false;
            }
        }
        private void EnterLongitudeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int longitude;
            if (int.TryParse(EnterLongitudeBox.Text, out longitude))
            {
                InvalidInputBlock.Visibility = Visibility.Collapsed;
                EnterLongitudeBox.Foreground = Brushes.Black;
                EnableButton();
            }
            else
            {
                InvalidInputBlock.Visibility = Visibility.Visible;
                InvalidInputBlock.Text = "Longitude must be a number";
                EnterLongitudeBox.Foreground = Brushes.Red;
                StationEntityAddButton.IsEnabled = false;
            }
        }

        private void UpdateTotalSlotsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int stationID;
            if (int.TryParse(UpdateTotalSlotsBox.Text, out stationID))
            {
                InvalidInputBlock.Visibility = Visibility.Collapsed;
                UpdateTotalSlotsBox.Foreground = Brushes.Black;
                EnableButton();
            }
            else
            {
                InvalidInputBlock.Visibility = Visibility.Visible;
                InvalidInputBlock.Text = "Available slots must be a number";
                UpdateTotalSlotsBox.Foreground = Brushes.Red;
                StationEntityAddButton.IsEnabled = false;
            }
        }

        private void AvailableSlotsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int stationID;
            if (int.TryParse(AvailableSlotsBox.Text, out stationID))
            {
                InvalidInputBlock.Visibility = Visibility.Collapsed;
                AvailableSlotsBox.Foreground = Brushes.Black;
                EnableButton();
            }
            else
            {
                InvalidInputBlock.Visibility = Visibility.Visible;
                InvalidInputBlock.Text = "Available slots must be a number";
                AvailableSlotsBox.Foreground = Brushes.Red;
                StationEntityAddButton.IsEnabled = false;
            }
        }
    }
}
