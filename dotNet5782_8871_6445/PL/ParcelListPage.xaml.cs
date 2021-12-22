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
        private BlApi.IBL BLW;
        private Frame Frame;
        CollectionView view;
        public ParcelListPage(BlApi.IBL IBL, Frame frame)
        {
            InitializeComponent();
            BLW = IBL;
            Frame = frame;

            ParcelDataGrid.ItemsSource = BLW.GetAllParcels();
            //ParcelListView.ItemsSource = BLW.GetAllParcels();
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            
        }

        private void AddNewParcelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = new ParcelPage(BLW, e, Frame);
        }

        private void BackWindow_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();// TO DO: to find a way to go back to main window without openning it again
        }

        private void ParcelListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParcelDataGrid.ItemsSource = BLW.GetParcels((Priorities)e.AddedItems[0], (WeightCategories)WeightSelector.SelectedIndex);
            //ParcelListView
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParcelDataGrid.ItemsSource = BLW.GetParcels((Priorities)PrioritySelector.SelectedIndex, (WeightCategories)e.AddedItems[0]);
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
                Frame.Content = new ParcelPage(BLW, item, Frame);
        }
        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelDataGrid.ItemsSource);
            //ParcelListView
            string str = e.OriginalSource.ToString();
            PropertyGroupDescription groupDescription = new PropertyGroupDescription(str);//TO DO: to make grouping possible for multipole definitions
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(groupDescription);
        }
    }
}
