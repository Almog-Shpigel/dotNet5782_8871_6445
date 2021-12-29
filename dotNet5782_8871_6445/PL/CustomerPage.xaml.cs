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
        private CustomerToList Customer;
        private RoutedEventArgs e;

        public CustomerPage(CustomerToList customer) //update ctor
        {
            InitializeComponent();
            Customer = customer;
            CustomerBL = IBL.GetCustomer(Customer.ID);
            DataContext = CustomerBL;
            ParcelSentListViewFromCustomer.ItemsSource = CustomerBL.ParcelesSentByCustomer;
            ParcelSentListViewFromCustomer.ItemsSource = CustomerBL.ParcelesSentToCustomer;
            AddNewCustomerPanell.Visibility = Visibility.Collapsed;
        }

        public CustomerPage(RoutedEventArgs e) //add ctor
        {
            InitializeComponent();
            ShowDetailsCustomer.Visibility = Visibility.Collapsed;
            ShowParcelsList.Visibility = Visibility.Collapsed;
            UpdateNameButton.Visibility = Visibility.Collapsed;
            UpdatePhoneButton.Visibility = Visibility.Collapsed;
            PrintLattitudeBlock.Text = "Lattitude:";
            PrintLongitudeBlock.Text = "Longitude:";
        }

        private void UpdateNameButton_Click(object sender, RoutedEventArgs e)
        {
            IBL.UpdateCustomerName(Convert.ToInt32(IDBlock.Text), NameBlock.Text);
            DataContext = IBL.GetCustomer(Customer.ID);
        }

        private void UpdatePhoneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IBL.UpdateCustomerPhone(Customer.ID, Convert.ToInt32(PhoneNumberBlock.Text));
                DataContext = IBL.GetCustomer(Customer.ID);
            }
            catch (Exception exp)
            {
                InvalidInputBlock.Text = exp.Message;
                InvalidInputBlock.Visibility = Visibility.Visible;
            }

        }

        private void EnterCustomerIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int customerID;
            if (!int.TryParse(EnterCustomerIDBox.Text, out customerID))
            {
                InvalidInputBlock.Visibility = Visibility.Visible;
                InvalidInputBlock.Text = "Customer id must contain only numbers";
                EnterCustomerIDBox.Foreground = Brushes.Red;
                CustomerEntityAddButton.IsEnabled = false;
            }
            else
            {
                InvalidInputBlock.Visibility = Visibility.Collapsed;
                EnterCustomerIDBox.Foreground = Brushes.Black;
                EnableButton();
            }
        }
        private void EnterCustomerNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }
        private void CustomerEntityAddButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerBL NewCustomer = new(Convert.ToInt32(EnterCustomerIDBox.Text), EnterCustomerNameBox.Text, EnterCustomerPhoneBox.Text,
               new(Convert.ToDouble(EnterLattitudeBox.Text), Convert.ToDouble(EnterLongitudeBox.Text)));
            try
            {
                IBL.AddNewCustomer(NewCustomer);
                CustomerEntityAddButton.IsEnabled = false;
                InvalidInputBlock.Text = "Customer added!";
                InvalidInputBlock.Visibility = Visibility.Visible;
                InvalidInputBlock.Foreground = Brushes.Green;
            }
            catch (Exception exp)
            {
                InvalidInputBlock.Text = exp.Message;
                InvalidInputBlock.Visibility = Visibility.Visible;
                EnterCustomerIDBox.Foreground = Brushes.Red;
                CustomerEntityAddButton.IsEnabled = false;
            }
        }
        private void EnableButton()
        {
            if (EnterCustomerIDBox.Text != "" && EnterCustomerNameBox.Text != ""
               && EnterCustomerPhoneBox.Text != "" && EnterLattitudeBox.Text != "" &&
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
        private void ParcelListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void EnterCustomerPhoneBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }
        private void EnterLattitudeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }
        private void EnterLongitudeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }
        private void UpdatePhoneBlock_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
