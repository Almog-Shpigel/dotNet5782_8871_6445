﻿using BL;
using BlApi;
using BO;
using DO;
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
        private BlApi.IBL IBL = BlFactory.GetBl();
        private BO.DroneBL droneBL;
        BackgroundWorker worker;

        public DronePage(RoutedEventArgs e)
        {
            InitializeComponent(); // Add drone ctor
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StationSelector.ItemsSource = IBL.GetAvailableStations().Select(station => (string)station.ID.ToString());
            DroneEntityAddButton.IsEnabled = false;
            UpdateLayout();
        }

        public DronePage(int DroneID) // Update drone ctor
        {
            InitializeComponent();
            droneBL = IBL.GetDrone(DroneID);
            DataContext = droneBL;
            ButtenEnableCheck();
        }
        
        private void updateDrone()
        {
            droneBL = IBL.GetDrone(int.Parse(IDBox.Text));
            batteryBorder.Width = droneBL.BatteryStatus;
            batteryBlock.Text = droneBL.BatteryStatus.ToString();
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

        //private void UpdateNameButton_Click(object sender, RoutedEventArgs e)
        //{
        //    IBL.UpdateDroneName(Convert.ToInt32(IDBlock.Text), ModelBlock.Text);
        //    ModelBlock.Text = IBL.GetDrone(Convert.ToInt32(IDBlock.Text)).Model;
        //}

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

        private void UpdateDroneToBeChargedButton_Click(object sender, RoutedEventArgs e)
        {
            InvalidBatteryToCompleteDeliveryBlock.Visibility = Visibility.Collapsed;
            try
            {
                IBL.UpdateDroneToBeCharged(Convert.ToInt32(IDBox.Text));
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
            IBL.UpdateDroneToBeAvailable(Convert.ToInt32(IDBox.Text));
            droneBL = IBL.GetDrone(droneBL.ID);
            DataContext = droneBL;
            ButtenEnableCheck();
        }

        private void UpdateParcelAssignToDroneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IBL.UpdateParcelAssignToDrone(Convert.ToInt32(IDBox.Text));
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
            IBL.UpdateParcelCollectedByDrone(Convert.ToInt32(IDBox.Text));
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

        private void DroneEntityAddButton_Click(object sender, RoutedEventArgs e)
        {
            int DroneID = Convert.ToInt32(IDBox.Text),
                StationID = Convert.ToInt32(StationSelector.Text);
            string name = ModelBlock.Text;
            WeightCategories weight = (WeightCategories)WeightSelector.SelectedItem;
            DroneBL drone = new(DroneID, name, weight);
            try
            {
                IBL.AddNewDrone(drone, StationID);
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
                //DroneEntityAddButton.IsEnabled = false;
            }
            catch (InvalidInputException exp)
            {
                InvalidDroneIDBlock.Text = exp.Message;
                InvalidDroneIDBlock.Visibility = Visibility.Visible;
                IDBox.Foreground = Brushes.Red;
                //DroneEntityAddButton.IsEnabled = false;
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
            IBL.UpdateDroneName(Convert.ToInt32(IDBox.Text), ModelBlock.Text);
            ModelBlock.Text = IBL.GetDrone(Convert.ToInt32(IDBox.Text)).Model;
        }

        private void PreviewParcel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void PreviewInMap_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("This feature is not implemented yet", "TBD", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void updateView()
        {
            worker.ReportProgress(0);
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
            worker.RunWorkerAsync();
        }

        private void ManualThreadButton_Click(object sender, RoutedEventArgs e)
        {

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
            IBL.UpdateDroneSimulatorStart(droneBL.ID, updateView, checkIfCanceled);
        }

    }
}
