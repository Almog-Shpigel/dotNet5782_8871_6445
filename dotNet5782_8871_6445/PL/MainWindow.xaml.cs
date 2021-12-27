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
using BO;
using DO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlApi.IBL IBL;
        private DroneListPage droneListPage;
        private DronePage dronePage;

        public MainWindow()
        {
            IBL = BlFactory.GetBl();
            InitializeComponent();
        }

        #region Drone
        private void DroneListPageButton_Click(object sender, RoutedEventArgs e)
        {
            droneListPage = new();
            droneListPage.ListAddButton.Click += ListAddButton_Click;
            droneListPage.DronesListView.SelectionChanged += DronesListView_SelectionChanged;
            droneListPage.DronesListView.PreviewMouseLeftButtonDown += DronesListView_PreviewMouseLeftButtonDown; ;
            droneListPage.BackWindow.Click += BackWindow_Click;
            Content = droneListPage;
        }

        #region Add

        private void ListAddButton_Click(object sender, RoutedEventArgs e)
        {
            dronePage = new(e);
            dronePage.EntityAddButton.Click += EntityAddButton_Click;
            Content = dronePage;
        }

        private void EntityAddButton_Click(object sender, RoutedEventArgs e)
        {
            DroneListPage droneListPage = new();
            int DroneID = Convert.ToInt32(dronePage.EnterDroneIDBox.Text), StationID = Convert.ToInt32(dronePage.StationSelector.Text);
            string name = dronePage.EnterModelNameBox.Text;
            WeightCategories weight = (WeightCategories)dronePage.WeightSelector.SelectedItem;
            DroneBL drone = new(DroneID, name, weight);
            try
            {
                IBL.AddNewDrone(drone, StationID);
                dronePage.EnterDroneIDBox.IsEnabled = false;
                dronePage.EnterModelNameBox.IsEnabled = false;
                dronePage.StationSelector.IsEnabled = false;
                dronePage.WeightSelector.IsEnabled = false;
                dronePage.EntityAddButton.IsEnabled = false;
                Content = droneListPage;
            }
            catch (InvalidIDException exp)
            {
                dronePage.InvalidDroneIDBlock.Text = exp.Message;
                dronePage.InvalidDroneIDBlock.Visibility = Visibility.Visible;
                dronePage.EnterDroneIDBox.Foreground = Brushes.Red;
                dronePage.EntityAddButton.IsEnabled = false;
            }
            catch (DroneExistExceptionBL exp)
            {
                dronePage.InvalidDroneIDBlock.Text = exp.Message;
                dronePage.InvalidDroneIDBlock.Visibility = Visibility.Visible;
                dronePage.EnterDroneIDBox.Foreground = Brushes.Red;
                dronePage.EntityAddButton.IsEnabled = false;
            }
            catch (StationExistExceptionBL exp)
            {
                dronePage.InvalidStationIDBlock.Text = exp.Message;
                dronePage.InvalidStationIDBlock.Visibility = Visibility.Visible;
                dronePage.EntityAddButton.IsEnabled = false;
            }
            catch (InvalidSlotsException exp)
            {
                dronePage.InvalidStationIDBlock.Text = exp.Message;
                dronePage.InvalidStationIDBlock.Visibility = Visibility.Visible;
                dronePage.EntityAddButton.IsEnabled = false;
            }
        }
        #endregion

        #region Update
        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneToList drone = (DroneToList)e.AddedItems[0];
            if (drone != null)
            {
                dronePage = new(drone.ID);
                dronePage.GoBackDroneListPage.Click += GoBackDroneListPage_Click;
                Content = dronePage;
            }
        }

        private void DronesListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //DroneToList drone = (DroneToList)e.OriginalSource;
            //if (drone != null)
            //{
            //    dronePage = new(drone.ID);
            //    dronePage.GoBackDroneListPage.Click += GoBackDroneListPage_Click;
            //    Content = dronePage;
            //}
        }

        private void GoBackDroneListPage_Click(object sender, RoutedEventArgs e)
        {
            droneListPage = new();
            Content = droneListPage;
        }
        #endregion
        #endregion

        private void BackWindow_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        

        

        private void StationListPageButton_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new StationListPage(Main);
        }

        private void CustomerListPageButton_Click(object sender, RoutedEventArgs e)
        {
            //Main.Content = new CustomerListPage(IBL);
        }

        private void ParcelListPageButton_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new ParcelListPage(Main);
        }
        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            new MapWindow().Show();
            Close();
        }
    }
}
