using BL;
using BlApi;
using BO;
using DO;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.ComponentModel;
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
        private BlApi.IBL bl = BlFactory.GetBl();
        private BO.DroneBL droneBL;
        BackgroundWorker worker;
        Pushpin pin;

        public DronePage(RoutedEventArgs e)
        {
            InitializeComponent(); // Add drone ctor
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StationSelector.ItemsSource = bl.GetAvailableStations().Select(station => (string)station.ID.ToString());
            DroneEntityAddButton.IsEnabled = false;
            UpdateButtonsPanel.Visibility = Visibility.Collapsed;
            InvalidDroneIDBlock.Visibility = Visibility.Collapsed;
            IDBox.IsReadOnly = false;
            UpdateLayout();
        }

        public DronePage(int DroneID) // Update drone ctor
        {
            InitializeComponent();
            droneBL = bl.GetDrone(DroneID);
            DataContext = droneBL;
            DroneEntityAddButton.Visibility = Visibility.Collapsed;
            ButtenEnableCheck();
            pin = new();
            ToolTip tt = new();
            pin.Location = new(droneBL.CurrentLocation.Latitude, droneBL.CurrentLocation.Longitude);
            pin.Tag = droneBL.ID;
            tt.Content = droneBL.ToString();
            pin.ToolTip = tt;
            myMap.Children.Add(pin);
            myMap.Center = pin.Location;
        }


        private void ButtenEnableCheck()
        {
            UpdateDroneToBeChargedButton.IsEnabled = false;
            UpdateReleaseDroneFromChargeButton.IsEnabled = false;
            UpdateParcelAssignToDroneButton.IsEnabled = false;
            UpdateParcelCollectedByDroneButton.IsEnabled = false;
            UpdateParcelDeleiveredByDroneButton.IsEnabled = false;
            PreviewParcel.IsEnabled = true;
            if (droneBL.Status == EnumsBL.DroneStatus.Charging)
            {
                UpdateReleaseDroneFromChargeButton.IsEnabled = true;
                PreviewParcel.IsEnabled = false;
                return;
            }
            if (droneBL.Status == EnumsBL.DroneStatus.Available)
            {
                UpdateDroneToBeChargedButton.IsEnabled = true;
                UpdateParcelAssignToDroneButton.IsEnabled = true;
                PreviewParcel.IsEnabled = false;
                return;
            }
            
            ParcelBL parcel = bl.GetParcel(droneBL.Parcel.ID);
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

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void EnableButton()
        {

            if (InvalidDroneIDBlock.Visibility != Visibility.Visible &&
                IDBox.Text != "" &&
                WeightSelector.SelectedIndex != -1 && StationSelector.SelectedIndex != -1)
                DroneEntityAddButton.IsEnabled = true;
            else
                DroneEntityAddButton.IsEnabled = false;
        }

        private void UpdateDroneToBeChargedButton_Click(object sender, RoutedEventArgs e)
        {
            InvalidBatteryToCompleteDeliveryBlock.Visibility = Visibility.Collapsed;
            try
            {
                bl.UpdateDroneToBeCharged(Convert.ToInt32(IDBox.Text));
                droneBL = bl.GetDrone(droneBL.ID);
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
            try 
            {
                bl.UpdateDroneToBeAvailable(Convert.ToInt32(IDBox.Text));
                droneBL = bl.GetDrone(droneBL.ID);
                DataContext = droneBL;
                ButtenEnableCheck();
            }
            catch (Exception exp)
            {
                InvalidBatteryToCompleteDeliveryBlock.Text = exp.Message;
                InvalidBatteryToCompleteDeliveryBlock.Visibility = Visibility.Visible;
            }
            
        }

        private void UpdateParcelAssignToDroneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateParcelAssignToDrone(Convert.ToInt32(IDBox.Text));
                droneBL = bl.GetDrone(droneBL.ID);
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
            bl.UpdateParcelCollectedByDrone(Convert.ToInt32(IDBox.Text));
            droneBL = bl.GetDrone(droneBL.ID);
            DataContext = droneBL;
            ButtenEnableCheck();
        }

        private void UpdateParcelDeleiveredByDroneButton_Click(object sender, RoutedEventArgs e)
        {
            bl.UpdateParcelDeleiveredByDrone(droneBL.ID);
            droneBL = bl.GetDrone(droneBL.ID);
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

        private void DroneEntityAddButton_Click(object sender, RoutedEventArgs e)
        {
            int DroneID = Convert.ToInt32(IDBox.Text),
                StationID = Convert.ToInt32(StationSelector.Text);
            string name = ModelBlock.Text;
            WeightCategories weight = (WeightCategories)WeightSelector.SelectedItem;
            DroneBL drone = new(DroneID, name, weight);
            try
            {
                bl.AddNewDrone(drone, StationID);
                IDBox.IsEnabled = false;
                StationSelector.IsEnabled = false;
                WeightSelector.IsEnabled = false;
                DroneEntityAddButton.IsEnabled = false;
                InvalidDroneIDBlock.Text = "Drone added!";
                InvalidDroneIDBlock.Visibility = Visibility.Visible;
                InvalidDroneIDBlock.Foreground = Brushes.Green;
            }
            catch (InvalidIDException exp)
            {
                InvalidDroneIDBlock.Text = exp.Message;
                InvalidDroneIDBlock.Visibility = Visibility.Visible;
                IDBox.Foreground = Brushes.Red;
               
            }
            catch (InvalidInputException exp)
            {
                InvalidDroneIDBlock.Text = exp.Message;
                InvalidDroneIDBlock.Visibility = Visibility.Visible;
                IDBox.Foreground = Brushes.Red;
                
            }
        }

        private void DroneListGoBackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateNameButton_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Are you sure you want change the model name?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res == MessageBoxResult.No)
                return;
            bl.UpdateDroneName(Convert.ToInt32(IDBox.Text), ModelBlock.Text);
            ModelBlock.Text = bl.GetDrone(Convert.ToInt32(IDBox.Text)).Model;
        }

        private void PreviewParcel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void PreviewInMap_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("This feature is not implemented yet", "TBD", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void updateDrone()
        {
            droneBL = bl.GetDrone(droneBL.ID);
            batteryBorder.Width = droneBL.BatteryStatus;
            batteryBlock.Text = droneBL.BatteryStatus.ToString();
            StatusBlock.Text = droneBL.Status.ToString();
            ParcelBlock.Text = droneBL.Parcel.ID.ToString();
            LocationBlock.Text = droneBL.CurrentLocation.ToString();
            myMap.Children.Remove(pin);
            pin = new();
            ToolTip tt = new();
            pin.Location = new(droneBL.CurrentLocation.Latitude, droneBL.CurrentLocation.Longitude);
            pin.Tag = droneBL.ID;
            tt.Content = droneBL.ToString();
            pin.ToolTip = tt;
            myMap.Children.Add(pin);
        }

        private bool checkIfCanceled()
        {
            return worker.CancellationPending;
        }

        private void AutoThreadButton_Click(object sender, RoutedEventArgs e)
        {
            worker = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true, };
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync(droneBL.ID);
            UpdateButtonsPanel.IsEnabled = false;
        }

        private void updateView()
        {
            worker.ReportProgress(0);
        }

        private void ManualThreadButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateButtonsPanel.IsEnabled = true;
            worker.CancelAsync();
            ButtenEnableCheck();
            updateDrone();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            updateDrone();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            updateDrone();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bl.UpdateDroneSimulatorStart((int)e.Argument, updateView, checkIfCanceled);
        }

        private void IDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int droneID;
            if (!int.TryParse(IDBox.Text, out droneID))
            {
                InvalidDroneIDBlock.Visibility = Visibility.Visible;
                IDBox.Foreground = Brushes.Red;
                DroneEntityAddButton.IsEnabled = false;
            }
            else
            {
                InvalidDroneIDBlock.Visibility = Visibility.Collapsed;
                IDBox.Foreground = Brushes.Black;
                EnableButton();
            }
        }
    }
}
