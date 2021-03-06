using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BL;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        private IBL IBL = BlFactory.GetBl();
        private CustomerBL CustomerBL;

        public CustomerPage(int customerID) //update ctor
        {
            InitializeComponent();
            CustomerBL = IBL.GetCustomer(customerID);
            DataContext = CustomerBL;
            CustomerEntityAddButton.Visibility = Visibility.Collapsed;
            IconEntityAdded.Visibility = Visibility.Collapsed;
            ParcelSentListViewFromCustomer.ItemsSource = CustomerBL.ParcelesSentByCustomer;
            ParcelReceivedListViewFromCustomer.ItemsSource = CustomerBL.ParcelesSentToCustomer;
        }

        public CustomerPage(RoutedEventArgs e) //add ctor
        {
            InitializeComponent();
            UpdateNameButton.Visibility = Visibility.Collapsed;
            IconEntityAdded.Visibility = Visibility.Collapsed;
            IDBox.IsReadOnly = false;
        }

        private void UpdateNameButton_Click(object sender, RoutedEventArgs e)
        {
            IBL.UpdateCustomerName(Convert.ToInt32(IDBox.Text), NameBox.Text);
            DataContext = IBL.GetCustomer(CustomerBL.ID);
        }

        private void UpdatePhoneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IBL.UpdateCustomerPhone(CustomerBL.ID, Convert.ToInt32(PhoneNumberBox.Text));
                DataContext = IBL.GetCustomer(CustomerBL.ID);
            }
            catch (Exception exp)
            {
                InvalidInputBlock.Text = exp.Message;
                InvalidInputBlock.Visibility = Visibility.Visible;
            }

        }

        private void CustomerEntityAddButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerBL NewCustomer = new(Convert.ToInt32(IDBox.Text), NameBox.Text, PhoneNumberBox.Text,
               new(Convert.ToDouble(EnterLatitudeBox.Text), Convert.ToDouble(EnterLongitudeBox.Text)));
            try
            {
                IBL.AddNewCustomer(NewCustomer);
                CustomerEntityAddButton.IsEnabled = false;
                InvalidInputBlock.Text = "Customer added!";
                InvalidInputBlock.Visibility = Visibility.Visible;
                InvalidInputBlock.Foreground = Brushes.Green;
                IDBox.IsEnabled = false;
                NameBox.IsEnabled = false;
                PhoneNumberBox.IsEnabled = false;
                EnterLatitudeBox.IsEnabled = false;
                EnterLongitudeBox.IsEnabled = false;
                IconEntityAdded.Visibility = Visibility.Visible;
            }
            catch (InvalidInputException exp)
            {
                InvalidInputBlock.Text = exp.Message;
                InvalidInputBlock.Visibility = Visibility.Visible;
                IDBox.Foreground = Brushes.Red;
                CustomerEntityAddButton.IsEnabled = false;
            }
        }
        private void EnableButton()
        {
            if (IDBox.Text != "" && NameBox.Text != ""
               && PhoneNumberBox.Text != "" && EnterLatitudeBox.Text != "" &&
               EnterLongitudeBox.Text != "" && InvalidInputBlock.Visibility != Visibility.Visible)
            {
                CustomerEntityAddButton.IsEnabled = true;
            }
            else
            {
                CustomerEntityAddButton.IsEnabled = false;
            }
        }
        private void CustomerListGoBackButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ParcelSentListViewFromCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void ParcelReceivedListViewFromCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void IDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(IDBox.Text, out int customerID))
            {
                InvalidInputBlock.Visibility = Visibility.Visible;
                InvalidInputBlock.Text = "Customer id must contain only numbers";
                IDBox.Foreground = Brushes.Red;
                CustomerEntityAddButton.IsEnabled = false;
            }
            else
            {
                InvalidInputBlock.Visibility = Visibility.Collapsed;
                IDBox.Foreground = Brushes.Black;
                EnableButton();
            }
        }

        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void PhoneNumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }
        private void EnterLatitudeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }
        private void EnterLongitudeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void UpdateNewParcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ParcelSentListViewFromCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ParcelSentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ParcelSentListViewFromCustomer_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ParcelReceivedListViewFromCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
