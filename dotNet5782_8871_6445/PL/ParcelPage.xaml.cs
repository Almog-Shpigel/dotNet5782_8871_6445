using BL;
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
        private DataGridRow item;
        private BlApi.IBL IBL = BlFactory.GetBl();
        private BO.ParcelToList Parcel;
        private BO.ParcelBL ParcelBL;
        Frame Frame;

        public ParcelPage(RoutedEventArgs e)
        {
            InitializeComponent(); // Add parcel ctor
            SenderIDSelector.ItemsSource = IBL.GetAllCustomerInParcels();
            TargetIDSelector.ItemsSource = SenderIDSelector.ItemsSource;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            PrintIDblock.Visibility = Visibility.Collapsed;
            ShowDetailsParcel2.Visibility = Visibility.Collapsed;
            EnableButton();
            UpdateLayout();
        }

        public ParcelPage(DataGridRow item) // Update parcel ctor
        {
            InitializeComponent();
            NewParcelEnterPanel.Visibility = Visibility.Collapsed;
            this.item = item;
            Parcel = (ParcelToList)item.DataContext;
            ParcelBL = this.IBL.GetParcel(Parcel.ID);
            DataContext = ParcelBL;
            
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
                AddNewParcelButton.IsEnabled = true;
            }
            else
            {
                AddNewParcelButton.IsEnabled = false;
            }
        }

        private void AddNewParcelButton_Click(object sender, RoutedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)Enum.Parse(typeof(WeightCategories), WeightSelector.Text);
            Priorities priority = (Priorities)Enum.Parse(typeof(Priorities), PrioritySelector.Text);
            CustomerInParcel Sender = (CustomerInParcel)SenderIDSelector.SelectedItem;
            CustomerInParcel Target = (CustomerInParcel)TargetIDSelector.SelectedItem;
            ParcelBL parcel = new(Sender, Target, weight, priority);
            try
            {
                IBL.AddNewParcel(parcel);
                NavigationService.GoBack();
            }
            catch (Exception) //TO DO: find a better Exception
            {
                AddNewParcelButton.IsEnabled = false;
            }
        }

        private void SenderIDSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void TargetIDSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
        }

        private void UpdateParcelDeliveredButton_Click(object sender, RoutedEventArgs e)
        {
            IBL.UpdateParcelDeleiveredByDrone(ParcelBL.DroneInParcel.ID);
            UpdateParcelDeliveredButton.IsEnabled = false;
        }
    }
}
