using BO;
using DO;
using System;
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
        private ListViewItem item;
        private BlApi.IBL BLW;
        private BO.ParcelToList Parcel;
        private BO.ParcelBL ParcelBL;
        Frame Frame;

        public ParcelPage(BlApi.IBL IBL, RoutedEventArgs e, Frame frame)
        {
            InitializeComponent(); // Add parcel ctor
            BLW = IBL;
            Frame = frame;
            SenderIDSelector.ItemsSource = BLW.GetAllCustomers().Select(customer => (string)customer.ID.ToString());
            TargetIDSelector.ItemsSource = BLW.GetAllCustomers();
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            PrintIDblock.Visibility = Visibility.Collapsed;
            EnableButton();
            UpdateLayout();
        }

        public ParcelPage(BlApi.IBL IBL, ListViewItem item, Frame frame) // Update parcel ctor
        {
            InitializeComponent();
            NewParcelEnterPanel.Visibility = Visibility.Collapsed;
            BLW = IBL;
            this.item = item;
            Frame = frame;
            Parcel = (ParcelToList)item.DataContext;
            ParcelBL = BLW.GetParcel(Parcel.ID);
            DataContext = ParcelBL;
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
            ParcelBL parcel = new(Convert.ToInt32(SenderIDSelector.Text), Convert.ToInt32(TargetIDSelector.Text), weight, priority);
            try
            {
                BLW.AddNewParcel(parcel);
                Frame.Content = new ParcelListPage(BLW, Frame);
            }
            catch (Exception) //TO DO: find a better Exception
            {
                AddNewParcelButton.IsEnabled = false;
            }
            Frame.Content = new ParcelListPage(BLW, Frame); //TO DO: erase if not needed
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

        }

        private void UpdateParcelDeliveredButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
