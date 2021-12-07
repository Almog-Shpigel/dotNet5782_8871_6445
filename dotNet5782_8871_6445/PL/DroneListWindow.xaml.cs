﻿using IDAL.DO;
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
using System.Windows.Shapes;
using static IBL.BO.EnumsBL;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        private IBL.BL BLW;
        public DroneListWindow(IBL.BL IBL)
        {
            InitializeComponent();
            BLW = IBL;
            DronesListView.ItemsSource = BLW.GetDrones(drone => true);
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = BLW.GetDrones(drone => drone.Status == (DroneStatus)e.AddedItems[0]);
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = BLW.GetDrones(drone => drone.MaxWeight == (WeightCategories)e.AddedItems[0]);
        }

        private void AddNewDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(BLW).Show();
        }

        private void ExitWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                Close();
            }
        }
    }
}
