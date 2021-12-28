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
        public CustomerPage(CustomerToList customer)
        {
            //InitializeComponent();
            //Customer = customer;
            //CustomerBL = IBL.GetCustomer(Customer.ID);
            //DataContext = CustomerBL;
            //ParcelsSentListViewFromStation.ItemsSource = CustomerBL.ParcelesSentByCustomer;
            //ParcelsReceivedListViewFromStation.ItemsSource = CustomerBL.ParcelesSentToCustomer;
            //AddNewCustomerPanell.Visibility = Visibility.Collapsed;
        }

        private void UpdateNameButton_Click(object sender, RoutedEventArgs e)
        {
            //IBL.UpdateCustomerName(Convert.ToInt32(IDBlock.Text), UpdateNameBlock.Text);
            //DataContext = IBL.GetStation(Customer.ID);
        }
    }
}
