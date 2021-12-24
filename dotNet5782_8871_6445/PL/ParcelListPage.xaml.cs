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
        private Frame Frame;
        CollectionView view;
        public ParcelListPage(Frame frame)
        {
            InitializeComponent();
            Frame = frame;

            ParcelDataGrid.ItemsSource = this.IBL.GetAllParcels();
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));            
        }

        private void AddNewParcelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = new ParcelPage(e);
        }

        private void BackWindow_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(null);
        }

        private void ParcelListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void DataGridRow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as DataGridRow;
            if (item != null && item.IsSelected)
            {
                Frame.Content = new ParcelPage(item);
            }
        }
    }
}
