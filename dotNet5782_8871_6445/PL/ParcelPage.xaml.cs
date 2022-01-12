using BL;
using BlApi;
using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelPage.xaml
    /// </summary>
    public partial class ParcelPage : Page
    {
        private readonly BlApi.IBL IBL = BlFactory.GetBl();
        public ParcelBL ParcelBL;

        public ParcelPage(RoutedEventArgs e) // Add parcel ctor
        {
            InitializeComponent(); 
            SenderIDSelector.ItemsSource = IBL.GetAllCustomerInParcels();
            TargetIDSelector.ItemsSource = SenderIDSelector.ItemsSource;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            UpdateParcelCollectedButton.Visibility = Visibility.Collapsed;
            EnableButton();
        }

        public ParcelPage(int ParcelID) // Update parcel ctor
        {
            InitializeComponent();
            ParcelBL = IBL.GetParcel(ParcelID);
            DataContext = ParcelBL;
            ParcelEntityAddButton.Visibility = Visibility.Collapsed;
            if (ParcelBL.Scheduled == null)
            {
                UpdateParcelCollectedButton.IsEnabled = false;
                UpdateParcelDeliveredButton.IsEnabled = false;
            }
            else if (ParcelBL.PickedUp == null && ParcelBL.Scheduled != null) 
            {
                UpdateParcelCollectedButton.IsEnabled = true;
                UpdateParcelDeliveredButton.IsEnabled = false;
            }
            else if (ParcelBL.Delivered == null && ParcelBL.PickedUp != null)
            {
                UpdateParcelCollectedButton.IsEnabled = false;
                UpdateParcelDeliveredButton.IsEnabled = true;
            }
            UpdateLayout();
        }

        private void EnableButton()
        {

            if (SenderIDSelector.SelectedIndex != -1
                && TargetIDSelector.SelectedIndex != -1
                && WeightSelector.SelectedIndex != -1
                && PrioritySelector.SelectedIndex != -1)
            {
                ParcelEntityAddButton.IsEnabled = true;
            }
            else
            {
                ParcelEntityAddButton.IsEnabled = false;
            }
        }

        private void ParcelEntityAddButton_Click(object sender, RoutedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)Enum.Parse(typeof(WeightCategories), WeightSelector.Text);
            Priorities priority = (Priorities)Enum.Parse(typeof(Priorities), PrioritySelector.Text);
            CustomerInParcel Sender = (CustomerInParcel)SenderIDSelector.SelectedItem;
            CustomerInParcel Target = (CustomerInParcel)TargetIDSelector.SelectedItem;
            ParcelBL parcel = new(Sender, Target, weight, priority);
            try
            {
                IBL.AddNewParcel(parcel);
                ParcelEntityAddButton.IsEnabled = false;
                SenderIDSelector.IsEnabled = false;
                TargetIDSelector.IsEnabled = false;
                WeightSelector.IsEnabled = false;
                PrioritySelector.IsEnabled = false;
            }
            catch (InvalidInputException ex)
            {
                InvalidTargetIDBlock.Visibility = Visibility.Visible;
                ParcelEntityAddButton.IsEnabled = false;
                InvalidTargetIDBlock.Text = ex.Message;
            }
        }

        private void SenderIDSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void TargetIDSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InvalidTargetIDBlock.Visibility = Visibility.Collapsed;
            EnableButton();
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void UpdateParcelCollectedButton_Click(object sender, RoutedEventArgs e)
        {
            IBL.UpdateParcelCollectedByDrone(ParcelBL.DroneInParcel.ID);
            UpdateParcelCollectedButton.IsEnabled = false;
            UpdateParcelDeliveredButton.IsEnabled = true;
            ParcelBL = IBL.GetParcel(ParcelBL.ID);
            DataContext = ParcelBL;
        }

        private void UpdateParcelDeliveredButton_Click(object sender, RoutedEventArgs e)
        {
            IBL.UpdateParcelDeleiveredByDrone(ParcelBL.DroneInParcel.ID);
            UpdateParcelDeliveredButton.IsEnabled = false;
            ParcelBL = IBL.GetParcel(ParcelBL.ID);
            DataContext = ParcelBL;
        }

        private void ParcelDataGridGoBackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateDeleteParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IBL.UpdateDeleteParcel(int.Parse(IDBlock.Text));
            }
            catch (BlApi.InvalidOperationException ex)
            {
                InvalidSenderIDBlock.Text = ex.Message;
                InvalidSenderIDBlock.Visibility = Visibility.Visible;
            }
        }

        private void PreviewSender_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void PreviewTarget_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void PreviewDroneInParcel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void DroneDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGridRow_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
