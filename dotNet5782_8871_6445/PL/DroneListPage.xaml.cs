﻿using DO;
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
        private BlApi.IBL BLW;
        private Frame MainFrame;
        public DroneListPage(BlApi.IBL IBL, Frame Main)
        {
            InitializeComponent();
            BLW = IBL;
            MainFrame = Main;
            DronesListView.ItemsSource = BLW.GetAllDrones();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = BLW.GetDrones((DroneStatus)e.AddedItems[0], (WeightCategories)WeightSelector.SelectedIndex);
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = BLW.GetDrones((DroneStatus)StatusSelector.SelectedIndex, (WeightCategories)e.AddedItems[0]);
        }

        private void AddNewDroneButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new DronePage(BLW, e, MainFrame);
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                MainFrame.Content = new DronePage(BLW,MainFrame, item);
            }
        }

        private void BackWindow_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Visibility = Visibility.Collapsed;
            new MainWindow().Show();// TO DO: to find a way to go back to main window without openning it again

        }
    }
}
