using BO;
using DO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for DronePage.xaml
    /// </summary>
    public partial class DronePage : Page
    {
        private ListViewItem item;
        private BlApi.IBL BLW;
        private BO.DroneToList Drone;
        private BO.DroneBL droneBL;
        Frame Frame;

        public DronePage(BlApi.IBL IBL, RoutedEventArgs e, Frame frame)
        {
            InitializeComponent(); // Add drone ctor
            BLW = IBL;
            Frame = frame;
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
            //StationSelector.ItemsSource = BLW.GetAllAvailableStationsID();
            AddNewDroneButton.IsEnabled = false;
            UpdateLayout();
        }

        public DronePage(BlApi.IBL IBL, ListViewItem item) // Update drone ctor
        {
            InitializeComponent();
            BLW = IBL;
            this.item = item;
            Drone = (BO.DroneToList)item.DataContext;
            NewDroneEnterPanel.Visibility = Visibility.Collapsed;
            PrintStationIDBlock.Visibility = Visibility.Collapsed;
            InvalidDroneIDBlock.Visibility = Visibility.Collapsed;
            InvalidStationIDBlock.Visibility = Visibility.Collapsed;
            InvalidBatteryToCompleteDeliveryBlock.Visibility = Visibility.Collapsed;
            ExistsDroneIDBlock.Visibility = Visibility.Collapsed;
            DisplayDroneDetailes();
            ButtenEnableCheck();
            UpdateLayout();
        }

        private void DisplayDroneDetailes()
        {
            droneBL = BLW.GetDrone(Drone.ID);
            IDBlock.Text = droneBL.ID.ToString();
            ModelBlock.Text = droneBL.Model;
            WeightBlock.Text = droneBL.MaxWeight.ToString();
            BatteryBlock.Text = droneBL.BatteryStatus.ToString();
            StatusBlock.Text = droneBL.Status.ToString();
            ParcelBlock.Text = droneBL.Parcel.ID.ToString();
            LocationBlock.Text = droneBL.CurrentLocation.ToString();
        }

        private void ButtenEnableCheck()
        {
            UpdateDroneToBeChargedButton.IsEnabled = false;
            UpdateReleaseDroneFromChargeButton.IsEnabled = false;
            UpdateParcelAssignToDroneButton.IsEnabled = false;
            UpdateParcelCollectedByDroneButton.IsEnabled = false;
            UpdateParcelDeleiveredByDroneButton.IsEnabled = false;

            if (Drone.Status == EnumsBL.DroneStatus.Charging)
            {
                UpdateReleaseDroneFromChargeButton.IsEnabled = true;
                return;
            }
            if (Drone.Status == EnumsBL.DroneStatus.Available)
            {
                UpdateDroneToBeChargedButton.IsEnabled = true;
                UpdateParcelAssignToDroneButton.IsEnabled = true;
                return;
            }

            ParcelBL parcel = BLW.GetParcel(Drone.ParcelID);
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
            BLW.UpdateDroneName(Convert.ToInt32(IDBlock.Text), UpdateNameBlock.Text);
            ModelBlock.Text = BLW.GetDrone(Convert.ToInt32(IDBlock.Text)).Model;
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
            WeightCategories weight = (WeightCategories)Enum.Parse(typeof(WeightCategories), WeightSelector.Text);
            DroneBL drone = new(Convert.ToInt32(EnterDroneIDBox.Text), EnterModelNameBox.Text, weight);
            try
            {
                BLW.AddNewDrone(drone, Convert.ToInt32(StationSelector.Text));
                EnterDroneIDBox.IsEnabled = false;
                EnterModelNameBox.IsEnabled = false;

                StationSelector.IsEnabled = false;
                WeightSelector.IsEnabled = false;
                AddNewDroneButton.IsEnabled = false;
                Frame.Content = new DroneListPage(BLW, Frame);
            }
            catch (InvalidIDException exp)
            {
                InvalidDroneIDBlock.Text = exp.Message;
                InvalidDroneIDBlock.Visibility = Visibility.Visible;
                EnterDroneIDBox.Foreground = Brushes.Red;
                AddNewDroneButton.IsEnabled = false;
            }
            catch (DroneExistExceptionBL exp)
            {
                InvalidDroneIDBlock.Text = exp.Message;
                InvalidDroneIDBlock.Visibility = Visibility.Visible;
                EnterDroneIDBox.Foreground = Brushes.Red;
                AddNewDroneButton.IsEnabled = false;
            }
            catch (StationExistExceptionBL exp)
            {
                InvalidStationIDBlock.Text = exp.Message;
                InvalidStationIDBlock.Visibility = Visibility.Visible;
                AddNewDroneButton.IsEnabled = false;
            }
            catch (InvalidSlotsException exp)
            {
                InvalidStationIDBlock.Text = exp.Message;
                InvalidStationIDBlock.Visibility = Visibility.Visible;
                AddNewDroneButton.IsEnabled = false;
            }
            
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
                BLW.UpdateDroneToBeCharged(Convert.ToInt32(IDBlock.Text));
                ButtenEnableCheck();
                DisplayDroneDetailes();
            }
            catch (Exception exp)
            {
                InvalidBatteryToCompleteDeliveryBlock.Text = exp.Message;
                InvalidBatteryToCompleteDeliveryBlock.Visibility = Visibility.Visible;
            }
        }

        private void UpdateReleaseDroneFromChargeButton_Click(object sender, RoutedEventArgs e)
        {
            BLW.UpdateDroneToBeAvailable(Convert.ToInt32(IDBlock.Text));
            ButtenEnableCheck();
            DisplayDroneDetailes();
        }

        private void UpdateParcelAssignToDroneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLW.UpdateParcelAssignToDrone(Convert.ToInt32(IDBlock.Text));
            }
            catch (Exception exp)
            {
                InvalidBatteryToCompleteDeliveryBlock.Visibility = Visibility.Visible;
                InvalidBatteryToCompleteDeliveryBlock.Text = exp.Message;
            }
            ButtenEnableCheck();
            DisplayDroneDetailes();
        }

        private void UpdateParcelCollectedByDroneButton_Click(object sender, RoutedEventArgs e)
        {
            BLW.UpdateParcelCollectedByDrone(Convert.ToInt32(IDBlock.Text));
            ButtenEnableCheck();
            DisplayDroneDetailes();
        }

        private void UpdateParcelDeleiveredByDroneButton_Click(object sender, RoutedEventArgs e)
        {
            BLW.UpdateParcelDeleiveredByDrone(Convert.ToInt32(IDBlock.Text));
            ButtenEnableCheck();
            DisplayDroneDetailes();
        }

        private void StationSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void EnterModelNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void UpdateDroneToBeChargedButton_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
