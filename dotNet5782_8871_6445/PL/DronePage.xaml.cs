using BL;
using BO;
using DO;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace PL
{
    /// <summary>
    /// Interaction logic for DronePage.xaml
    /// </summary>
    public partial class DronePage : Page
    {
        private BlApi.IBL IBL = BlFactory.GetBl();
        private BO.DroneBL droneBL;

        public DronePage(RoutedEventArgs e)
        {
            InitializeComponent(); // Add drone ctor
            ShowDetailsDrone1.Visibility = Visibility.Collapsed;
            ShowDetailsDrone2.Visibility = Visibility.Collapsed;
            UpdateNameBlock.Visibility = Visibility.Collapsed;
            PrintBatteryBlock.Visibility = Visibility.Collapsed;
            PrintStatusBlock.Visibility = Visibility.Collapsed;
            PrintParcelBlock.Visibility = Visibility.Collapsed;
            PrintLocationBlock.Visibility = Visibility.Collapsed;
            InvalidDroneIDBlock.Visibility = Visibility.Collapsed;
            InvalidStationIDBlock.Visibility = Visibility.Collapsed;
            InvalidBatteryToCompleteDeliveryBlock.Visibility = Visibility.Collapsed;
            ExistsDroneIDBlock.Visibility = Visibility.Collapsed;
            WeightCategories[] ARR = new WeightCategories[3];
            ARR[0] = WeightCategories.Light;
            ARR[1] = WeightCategories.Medium;
            ARR[2] = WeightCategories.Heavy;
            WeightSelector.ItemsSource = ARR;
            StationSelector.ItemsSource = IBL.GetAvailableStations().Select(station => (string)station.ID.ToString());
            AddNewDroneButton.IsEnabled = false;
            UpdateLayout();
        }

        public DronePage(int DroneID) // Update drone ctor
        {
            InitializeComponent();
            NewDroneEnterPanel.Visibility = Visibility.Collapsed;
            PrintStationIDBlock.Visibility = Visibility.Collapsed;
            InvalidDroneIDBlock.Visibility = Visibility.Collapsed;
            InvalidStationIDBlock.Visibility = Visibility.Collapsed;
            InvalidBatteryToCompleteDeliveryBlock.Visibility = Visibility.Collapsed;
            ExistsDroneIDBlock.Visibility = Visibility.Collapsed;
            droneBL = IBL.GetDrone(DroneID);
            this.DataContext = droneBL;
            ButtenEnableCheck();
            UpdateLayout();
        }


        private void ButtenEnableCheck()
        {
            UpdateDroneToBeChargedButton.IsEnabled = false;
            UpdateReleaseDroneFromChargeButton.IsEnabled = false;
            UpdateParcelAssignToDroneButton.IsEnabled = false;
            UpdateParcelCollectedByDroneButton.IsEnabled = false;
            UpdateParcelDeleiveredByDroneButton.IsEnabled = false;

            if (droneBL.Status == EnumsBL.DroneStatus.Charging)
            {
                UpdateReleaseDroneFromChargeButton.IsEnabled = true;
                return;
            }
            if (droneBL.Status == EnumsBL.DroneStatus.Available)
            {
                UpdateDroneToBeChargedButton.IsEnabled = true;
                UpdateParcelAssignToDroneButton.IsEnabled = true;
                return;
            }

            ParcelBL parcel = IBL.GetParcel(droneBL.Parcel.ID);
            if (parcel.PickedUp != null)
            {
                UpdateParcelDeleiveredByDroneButton.IsEnabled = true;
                return;
            }
            if (parcel.Scheduled != null)
            {
                UpdateParcelCollectedByDroneButton.IsEnabled = true;
                return;
            }
            if (parcel.TimeRequested != null)
                UpdateParcelCollectedByDroneButton.IsEnabled = true;
        }

        private void UpdateNameButton_Click(object sender, RoutedEventArgs e)
        {
            IBL.UpdateDroneName(Convert.ToInt32(IDBlock.Text), UpdateNameBlock.Text);
            ModelBlock.Text = IBL.GetDrone(Convert.ToInt32(IDBlock.Text)).Model;
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void EnableButton()
        {

            if (InvalidDroneIDBlock.Visibility != Visibility.Visible &&
                EnterDroneIDBox.Text != "" &&
                WeightSelector.SelectedIndex != -1 && StationSelector.SelectedIndex != -1)
                AddNewDroneButton.IsEnabled = true;
            else
                AddNewDroneButton.IsEnabled = false;
        }

        private void AddNewDroneButton_Click(object sender, RoutedEventArgs e)
        {
         
            
        }

        private void EnterDroneIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int droneID;
            if (!int.TryParse(EnterDroneIDBox.Text, out droneID))
            {
                InvalidDroneIDBlock.Visibility = Visibility.Visible;
                EnterDroneIDBox.Foreground = Brushes.Red;
                AddNewDroneButton.IsEnabled = false;
            }
            else
            {
                InvalidDroneIDBlock.Visibility = Visibility.Collapsed;
                EnterDroneIDBox.Foreground = Brushes.Black;
                EnableButton();
            }
        }

        private void UpdateDroneToBeChargedButton_Click(object sender, RoutedEventArgs e)
        {
            InvalidBatteryToCompleteDeliveryBlock.Visibility = Visibility.Collapsed;
            try
            {
                IBL.UpdateDroneToBeCharged(Convert.ToInt32(IDBlock.Text));
                droneBL = IBL.GetDrone(droneBL.ID);
                DataContext = droneBL;
                ButtenEnableCheck();
                
                
            }
            catch (Exception exp)
            {
                InvalidBatteryToCompleteDeliveryBlock.Text = exp.Message;
                InvalidBatteryToCompleteDeliveryBlock.Visibility = Visibility.Visible;
            }
        }

        private void UpdateReleaseDroneFromChargeButton_Click(object sender, RoutedEventArgs e)
        {
            IBL.UpdateDroneToBeAvailable(Convert.ToInt32(IDBlock.Text));
            droneBL = IBL.GetDrone(droneBL.ID);
            DataContext = droneBL;
            ButtenEnableCheck();
            
            
        }

        private void UpdateParcelAssignToDroneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IBL.UpdateParcelAssignToDrone(Convert.ToInt32(IDBlock.Text));
                droneBL = IBL.GetDrone(droneBL.ID);
                DataContext = droneBL;
                ButtenEnableCheck();
                
            }
            catch (Exception exp)
            {
                InvalidBatteryToCompleteDeliveryBlock.Visibility = Visibility.Visible;
                InvalidBatteryToCompleteDeliveryBlock.Text = exp.Message;
            }
            
            
        }

        private void UpdateParcelCollectedByDroneButton_Click(object sender, RoutedEventArgs e)
        {
            IBL.UpdateParcelCollectedByDrone(Convert.ToInt32(IDBlock.Text));
            droneBL = IBL.GetDrone(droneBL.ID);
            DataContext = droneBL;
            ButtenEnableCheck();
            
            
        }

        private void UpdateParcelDeleiveredByDroneButton_Click(object sender, RoutedEventArgs e)
        {
            IBL.UpdateParcelDeleiveredByDrone(droneBL.ID);
            droneBL = IBL.GetDrone(droneBL.ID);
            DataContext = droneBL;
            ButtenEnableCheck();
            
            
        }

        private void StationSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void EnterModelNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void DroneList_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
