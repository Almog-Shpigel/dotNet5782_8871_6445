using BL;
using DO;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using static BO.EnumsBL;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelListPage.xaml
    /// </summary>
    public partial class ParcelListPage : Page
    {
        private BlApi.IBL IBL = BlFactory.GetBl();

        public ParcelListPage()
        {
            InitializeComponent();
            ParcelDataGrid.ItemsSource = IBL.GetAllParcels();
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            SenderSelector.ItemsSource = IBL.GetAllCustomerInParcels();
            ReceiverSelector.ItemsSource = IBL.GetAllCustomerInParcels();
        }

        private void ParcelEntityAddButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BackWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ParcelDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParcelDataGrid.ItemsSource = IBL.GetParcels((Priorities)e.AddedItems[0], (WeightCategories)WeightSelector.SelectedIndex);
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParcelDataGrid.ItemsSource = IBL.GetParcels((Priorities)PrioritySelector.SelectedIndex, (WeightCategories)e.AddedItems[0]);
        }
        private void ReceiverSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // ParcelDataGrid.ItemsSource = IBL.GetParcels((CustomerInParcel)PrioritySelector.SelectedIndex, (WeightCategories)e.AddedItems[0]);
            
        }
        private void SenderSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ParcelDataGrid.ItemsSource = IBL.GetParcels((Priorities)PrioritySelector.SelectedIndex, (WeightCategories)e.AddedItems[0]);
     
        }
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ParcelDataGrid.ItemsSource = IBL.GetParcels((Priorities)PrioritySelector.SelectedIndex, (WeightCategories)e.AddedItems[0]);

        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ParcelDataGridAddButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
