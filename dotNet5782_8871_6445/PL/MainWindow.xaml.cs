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
        private StationListPage stationListPage;
        private StationPage stationPage;
        private CustomerListPage customerListPage;
        private CustomerPage customerPage;
        private ParcelListPage parcelListPage;
        private ParcelPage parcelPage;
        private MapWindow mapWindow;

        public MainWindow()
        {
            IBL = BlFactory.GetBl();
            InitializeComponent();
        }

        #region Drones
        private void DroneListPageButton_Click(object sender, RoutedEventArgs e)
        {
            droneListPage = new();
            droneListPage.DroneListAddButton.Click += DroneListAddButton_Click;
            droneListPage.BackWindow.Click += BackWindow_Click;
            droneListPage.DronesListView.MouseDoubleClick += DronesListView_MouseDoubleClick;
            Content = droneListPage;
        }

        #region Add
        private void DroneListAddButton_Click(object sender, RoutedEventArgs e)
        {
            dronePage = new(e);
            dronePage.DroneEntityAddButton.Click += DroneEntityAddButton_Click;
            Content = dronePage;
        }

        private void DroneEntityAddButton_Click(object sender, RoutedEventArgs e)
        {
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
                dronePage.DroneEntityAddButton.IsEnabled = false;
                Content = droneListPage;
            }
            catch (InvalidIDException exp)
            {
                dronePage.InvalidDroneIDBlock.Text = exp.Message;
                dronePage.InvalidDroneIDBlock.Visibility = Visibility.Visible;
                dronePage.EnterDroneIDBox.Foreground = Brushes.Red;
                dronePage.DroneEntityAddButton.IsEnabled = false;
            }
            catch (DroneExistExceptionBL exp)
            {
                dronePage.InvalidDroneIDBlock.Text = exp.Message;
                dronePage.InvalidDroneIDBlock.Visibility = Visibility.Visible;
                dronePage.EnterDroneIDBox.Foreground = Brushes.Red;
                dronePage.DroneEntityAddButton.IsEnabled = false;
            }
            catch (StationExistExceptionBL exp)
            {
                dronePage.InvalidStationIDBlock.Text = exp.Message;
                dronePage.InvalidStationIDBlock.Visibility = Visibility.Visible;
                dronePage.DroneEntityAddButton.IsEnabled = false;
            }
            catch (InvalidSlotsException exp)
            {
                dronePage.InvalidStationIDBlock.Text = exp.Message;
                dronePage.InvalidStationIDBlock.Visibility = Visibility.Visible;
                dronePage.DroneEntityAddButton.IsEnabled = false;
            }
        }
        #endregion

        #region Update
        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList drone = (DroneToList)droneListPage.DronesListView.SelectedItem;
            if (drone != null)
            {
                dronePage = new(drone.ID);
                dronePage.DroneListGoBackButton.Click += DroneListGoBackButton_Click;
                Content = dronePage;
            }
        }

        private void DroneListGoBackButton_Click(object sender, RoutedEventArgs e)
        {
            Content = droneListPage;
        }
        #endregion
        #endregion

        #region Stations
        private void StationListPageButton_Click(object sender, RoutedEventArgs e)
        {
            stationListPage = new();
            stationListPage.StationListAddButton.Click += StationListAddButton_Click;
            stationListPage.BackWindow.Click += BackWindow_Click;
            stationListPage.StationListView.MouseDoubleClick += StationListView_MouseDoubleClick;
            Content = stationListPage;
        }

        #region Add
        private void StationListAddButton_Click(object sender, RoutedEventArgs e)
        {
            stationPage = new(e);
            stationPage.StationEntityAddButton.Click += StationEntityAddButton_Click;
            stationPage.StationListGoBackButton.Click += StationListGoBackButton_Click;
            Content = stationPage;
        }

        private void StationEntityAddButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Update
        private void StationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StationToList station = (StationToList)stationListPage.StationListView.SelectedItem;
            if (station != null)
            {
                stationPage = new(station);
                stationPage.StationListGoBackButton.Click += StationListGoBackButton_Click;
                stationPage.DronesListViewFromStation.MouseDoubleClick += DronesListViewFromStation_MouseDoubleClick;
                Content = stationPage;
            }
        }

        private void DronesListViewFromStation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneChargeBL drone = (DroneChargeBL)stationPage.DronesListViewFromStation.SelectedItem;
            if (drone != null)
            {
                dronePage = new(drone.DroneID);
                dronePage.DroneListGoBackButton.Click += DroneListGoBackButton_Click;
                Content = dronePage;
            }
        }

        private void StationListGoBackButton_Click(object sender, RoutedEventArgs e)
        {
            Content = stationListPage;
        }
        #endregion
        #endregion

        #region Customers
        private void CustomerListPageButton_Click(object sender, RoutedEventArgs e)
        {
            //Main.Content = new CustomerListPage(IBL);
        }
        #endregion

        #region Parcels
        private void ParcelListPageButton_Click(object sender, RoutedEventArgs e)
        {
            //Main.Content = new ParcelListPage(Main);
        }
        #endregion

        #region Map
        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            new MapWindow().Show();
            Close();
        }
        #endregion

        private void BackWindow_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
