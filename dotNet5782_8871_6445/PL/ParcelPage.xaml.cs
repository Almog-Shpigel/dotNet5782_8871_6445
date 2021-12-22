using BO;
using DO;
using System;
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

        private void EnterSenderIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int senderID;
            if (!int.TryParse(EnterSenderIDBox.Text, out senderID))
            {
                InvalidSenderIDBlock.Visibility = Visibility.Visible;
                EnterSenderIDBox.Foreground = Brushes.Red;
                AddNewParcelButton.IsEnabled = false;
            }
            else
            {
                InvalidSenderIDBlock.Visibility = Visibility.Collapsed;
                EnterSenderIDBox.Foreground = Brushes.Black;
                EnableButton();
            }
        }

        private void EnterTargetIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int senderID;
            if (!int.TryParse(EnterSenderIDBox.Text, out senderID))
            {
                InvalidTargetIDBlock.Visibility = Visibility.Visible;
                EnterSenderIDBox.Foreground = Brushes.Red;
                AddNewParcelButton.IsEnabled = false;
            }
            else
            {
                InvalidTargetIDBlock.Visibility = Visibility.Collapsed;
                EnterSenderIDBox.Foreground = Brushes.Black;
                EnableButton();
            }
        }

        private void UpdateDeleteParcelButton_Click(object sender, RoutedEventArgs e)
        { }

        private void EnableButton()
        {

            if (InvalidSenderIDBlock.Visibility != Visibility.Visible &&
                InvalidTargetIDBlock.Visibility != Visibility.Visible &&
                EnterSenderIDBox.Text != "" &&
                EnterTargetIDBox.Text != "" &&
                WeightSelector.SelectedIndex != -1 &&
                PrioritySelector.SelectedIndex != -1)
                AddNewParcelButton.IsEnabled = true;
            else
                AddNewParcelButton.IsEnabled = false;
        }

        private void AddNewParcelButton_Click(object sender, RoutedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)Enum.Parse(typeof(WeightCategories), WeightSelector.Text);
            Priorities priority = (Priorities)Enum.Parse(typeof(Priorities), PrioritySelector.Text);
            ParcelBL parcel = new(Convert.ToInt32(EnterSenderIDBox.Text), Convert.ToInt32(EnterTargetIDBox.Text), weight, priority);
            try
            {
                BLW.AddNewParcel(parcel);
                Frame.Content = new ParcelListPage(BLW, Frame);
            }
            catch (InvalidIDException exp)
            {
                //InvalidParcelIDBlock.Text = exp.Message;
                //InvalidDroneIDBlock.Visibility = Visibility.Visible;
                //EnterDroneIDBox.Foreground = Brushes.Red;
                //AddNewParcelButton.IsEnabled = false;
            }
            catch (DroneExistExceptionBL exp)
            {
                //InvalidDroneIDBlock.Text = exp.Message;
                //InvalidDroneIDBlock.Visibility = Visibility.Visible;
                //EnterDroneIDBox.Foreground = Brushes.Red;
                //AddNewParcelButton.IsEnabled = false;
            }
            catch (StationExistExceptionBL exp)
            {
                //InvalidStationIDBlock.Text = exp.Message;
                //InvalidStationIDBlock.Visibility = Visibility.Visible;
                //AddNewParcelButton.IsEnabled = false;
            }
            catch (InvalidSlotsException exp)
            {
                //InvalidStationIDBlock.Text = exp.Message;
                //InvalidStationIDBlock.Visibility = Visibility.Visible;
                //AddNewParcelButton.IsEnabled = false;
            }
            Frame.Content = new ParcelListPage(BLW, Frame); //TO DO: erase
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
