using BL;
using DO;
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
using static BO.EnumsBL;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListPage.xaml
    /// </summary>
    public partial class DroneListPage : Page
    {
        private BlApi.IBL IBL = BlFactory.GetBl();
        public DroneListPage()
        {
            InitializeComponent();
            DronesListView.ItemsSource = IBL.GetAllDrones();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = IBL.GetDrones((DroneStatus)e.AddedItems[0], (WeightCategories)WeightSelector.SelectedIndex);
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = IBL.GetDrones((DroneStatus)StatusSelector.SelectedIndex, (WeightCategories)e.AddedItems[0]);
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var item = sender as ListViewItem;
            //if (item != null && item.IsSelected)
            //{
            //    BO.DroneToList drone = (BO.DroneToList)item.DataContext;
            //}
        }

        private void BackWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListAddButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
