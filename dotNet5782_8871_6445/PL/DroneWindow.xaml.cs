﻿using IBL.BO;
using IDAL.DO;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private ListViewItem item;
        private IBL.BL BLW;
        private IBL.BO.DroneToList Drone;

        public DroneWindow(IBL.BL IBL, RoutedEventArgs e)
        {
            InitializeComponent(); // Add drone ctor
            BLW = IBL;
            UpdateNameButton.Visibility = Visibility.Collapsed;
            UpdateNameBlock.Visibility = Visibility.Collapsed;
            UpdateDroneToBeChargedButton.Visibility = Visibility.Collapsed;
            UpdateReleaseDroneFromChargeButton.Visibility = Visibility.Collapsed;
            UpdateParcelAssignToDroneButton.Visibility = Visibility.Collapsed;
            UpdateParcelCollectedByDroneButton.Visibility = Visibility.Collapsed;
            UpdateParcelDeleiveredByDroneButton.Visibility = Visibility.Collapsed;
            PrintBatteryBlock.Visibility = Visibility.Collapsed;
            PrintStatusBlock.Visibility = Visibility.Collapsed;
            PrintParcelBlock.Visibility = Visibility.Collapsed;
            PrintLocationBlock.Visibility = Visibility.Collapsed;
            InvalidDroneIDBlock.Visibility = Visibility.Collapsed;
            InvalidStationIDBlock.Visibility = Visibility.Collapsed;
            ExistsDroneIDBlock.Visibility =Visibility.Collapsed;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            AddNewDroneButton.IsEnabled = false;
            UpdateLayout();
        }

        public DroneWindow(IBL.BL IBL, ListViewItem item) // Update drone ctor
        {
            InitializeComponent();
            BLW = IBL;
            this.item = item;
            Drone = (IBL.BO.DroneToList)item.DataContext;
            IBL.BO.DroneBL droneBL = BLW.DisplayDrone(Drone.ID);
            IDBlock.Text = droneBL.ID.ToString();
            ModelBlock.Text = droneBL.Model;
            WeightBlock.Text = droneBL.MaxWeight.ToString();
            BatteryBlock.Text = droneBL.BatteryStatus.ToString();
            StatusBlock.Text = droneBL.Status.ToString();
            ParcelBlock.Text = droneBL.Parcel.ID.ToString();
            LocationBlock.Text = droneBL.CurrentLocation.ToString();
            
            PrintStationIDBlock.Visibility = Visibility.Collapsed;
            EnterDroneIDBox.Visibility = Visibility.Collapsed;
            EnterModelNameBox.Visibility = Visibility.Collapsed;
            WeightSelector.Visibility = Visibility.Collapsed;
            EnterStationIDBox.Visibility = Visibility.Collapsed;
            AddNewDroneButton.Visibility = Visibility.Collapsed;
            InvalidDroneIDBlock.Visibility = Visibility.Collapsed;
            InvalidStationIDBlock.Visibility = Visibility.Collapsed;
            ExistsDroneIDBlock.Visibility = Visibility.Collapsed;
            ButtenEnableCheck();
            UpdateLayout();
        }

        private void ButtenEnableCheck()
        {
            ParcelBL parcel = BLW.DisplayParcel(Drone.ParcelID);
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
            if (parcel.PickedUp != null)
            {
                UpdateParcelDeleiveredByDroneButton.IsEnabled = true;
                return;
            }
            if(parcel.Scheduled != null)
            {
                UpdateParcelCollectedByDroneButton.IsEnabled = true;
                return;
            }
            if(parcel.TimeRequested != null)
                UpdateParcelCollectedByDroneButton.IsEnabled = true;
        }

        private void UpdateNameButton_Click(object sender, RoutedEventArgs e)
        {
            BLW.UpdateDroneName(Convert.ToInt32(IDBlock.Text), UpdateNameBlock.Text);
            // ModelBlock.Text = UpdateNameBlock.Text;
            // new DroneListWindow(BLW).Show();
            // Needs to change the button to inform the user the update accured
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void EnableButton()
        {
            if (InvalidDroneIDBlock.Visibility != Visibility.Visible &&
                EnterDroneIDBox.Text != "" &&
               InvalidStationIDBlock.Visibility != Visibility.Visible &&
               EnterStationIDBox.Text != "" &&
               WeightSelector.SelectedIndex != -1)
                AddNewDroneButton.IsEnabled = true;
        }

        private void AddNewDroneButton_Click(object sender, RoutedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)Enum.Parse(typeof(WeightCategories), WeightSelector.Text);
            DroneBL drone = new(Convert.ToInt32(EnterDroneIDBox.Text), EnterModelNameBox.Text, weight);
            try
            {
                BLW.AddNewDrone(drone, Convert.ToInt32(EnterStationIDBox.Text));
            }
            catch (Exception)
            {
                //ExistsDroneIDBlock.Text = exp.Message;
                //ExistsDroneIDBlock.Visibility = Visibility.Visible;
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

        private void EnterStationIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(EnterStationIDBox.Text, out int StationID))
            {
                InvalidStationIDBlock.Visibility = Visibility.Visible;
                EnterStationIDBox.Foreground = Brushes.Red;
                AddNewDroneButton.IsEnabled = false;
            }
            else
            {
                InvalidStationIDBlock.Visibility = Visibility.Collapsed;
                EnterStationIDBox.Foreground = Brushes.Black;
                EnableButton();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(BLW).Show();
            Close();
        }

        private void UpdateDroneToBeChargedButton_Click(object sender, RoutedEventArgs e)
        {
            BLW.UpdateDroneToBeCharged(Convert.ToInt32(IDBlock.Text));
            ButtenEnableCheck();
        }

        private void UpdateReleaseDroneFromChargeButton_Click(object sender, RoutedEventArgs e)
        {
            BLW.UpdateDroneAvailable(Convert.ToInt32(IDBlock.Text));
            ButtenEnableCheck();
        }

        private void UpdateParcelAssignToDroneButton_Click(object sender, RoutedEventArgs e)
        {
            BLW.UpdateParcelAssignToDrone(Convert.ToInt32(IDBlock.Text));
            ButtenEnableCheck();
        }

        private void UpdateParcelCollectedByDroneButton_Click(object sender, RoutedEventArgs e)
        {
            BLW.UpdateParcelCollectedByDrone(Convert.ToInt32(IDBlock.Text));
            ButtenEnableCheck();
        }

        private void UpdateParcelDeleiveredByDroneButton_Click(object sender, RoutedEventArgs e)
        {
            BLW.UpdateParcelDeleiveredByDrone(Convert.ToInt32(IDBlock.Text));
            ButtenEnableCheck();
        }
    }
}
