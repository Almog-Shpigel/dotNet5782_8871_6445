using BL;
using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //SenderSelector.ItemsSource = IBL.GetAllCustomerInParcels();
            //ReceiverSelector.ItemsSource = IBL.GetAllCustomerInParcels();
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
            SortParcels(); 
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortParcels();
        }
        //private void ReceiverSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //   // ParcelDataGrid.ItemsSource = IBL.GetParcels((CustomerInParcel)PrioritySelector.SelectedIndex, (WeightCategories)e.AddedItems[0]);
            
        //}
        //private void SenderSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //ParcelDataGrid.ItemsSource = IBL.GetParcels((Priorities)PrioritySelector.SelectedIndex, (WeightCategories)e.AddedItems[0]);
     
        //}
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortParcels();
        }
        private void From_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SortParcels();
        }

        private void To_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SortParcels();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ParcelDataGridAddButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void SortParcels()
        {
            ParcelDataGrid.ItemsSource = IBL.GetParcels((Priorities)PrioritySelector.SelectedIndex, (WeightCategories)WeightSelector.SelectedIndex, (ParcelStatus)StatusSelector.SelectedIndex, FromDatePicker.SelectedDate, ToDatePicker.SelectedDate);
        }

        private void ParcelDataGridClear_Click(object sender, RoutedEventArgs e)
        {
            FromDatePicker.SelectedDate = null;
            ToDatePicker.SelectedDate = null;
            WeightSelector.SelectedIndex = -1;
            StatusSelector.SelectedIndex = -1;
            PrioritySelector.SelectedIndex = -1;
            SortParcels();
        }

        private void GroupBySenderButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                IEnumerable<ParcelToList> parcelList = IBL.GetParcelsGroupBy("sender");
                ParcelDataGrid.ItemsSource = IBL.GetParcelsGroupBy("sender");
            }
            catch(Exception ex)
            {

            }
                      
        }

        private void GroupByReceiverButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ParcelDataGrid.ItemsSource = IBL.GetParcelsGroupBy("reciver");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
