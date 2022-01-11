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

        private void FilterSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = IBL.GetDrones((DroneStatus)StatusSelector.SelectedIndex, (WeightCategories)WeightSelector.SelectedIndex);
        }

       

        private void BackWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DroneListAddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedIndex = -1;
            WeightSelector.SelectedIndex = -1;
            DronesListView.ItemsSource = IBL.GetAllDrones();
        }
    }
}
