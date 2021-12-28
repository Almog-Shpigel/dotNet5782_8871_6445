using System;
using System.Windows;
using System.Windows.Input;
using BL;
using BO;

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
            ManagerPanel.Visibility = Visibility.Collapsed;
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

        private void DroneListAddButton_Click(object sender, RoutedEventArgs e)
        {
            dronePage = new(e);
            dronePage.DroneEntityAddButton.Click += DroneListPageButton_Click;
            
            Content = dronePage;
        }

        private void PreviewParcel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool success = int.TryParse(dronePage.ParcelBlock.Text, out int ParcelID);
            if (success)
            {
                parcelPage = new(ParcelID);
                parcelPage.ParcelDataGridGoBackButton.Click += ParcelListPageButton_Click;
                parcelPage.UpdateDeleteParcelButton.Click += ParcelListPageButton_Click;
                parcelPage.ParcelEntityAddButton.Click += ParcelListPageButton_Click;
                Content = parcelPage;
            }
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList drone = (DroneToList)droneListPage.DronesListView.SelectedItem;
            if(drone != null)
            {
                dronePage = new(drone.ID);
                dronePage.DroneListGoBackButton.Click += DroneListPageButton_Click;
                dronePage.PreviewParcel.MouseLeftButtonDown += PreviewParcel_MouseLeftButtonDown;
                Content = dronePage;
            }
        }

        //private void ParcelListViewFromDrone_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    DroneChargeBL drone = (DroneChargeBL)stationPage.DronesListViewFromStation.SelectedItem;
        //    if (drone != null)
        //    {
        //        dronePage = new(drone.DroneID);
        //        dronePage.DroneListGoBackButton.Click += DroneListPageButton_Click;
        //        Content = dronePage;
        //    }
        //}
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

        private void StationListAddButton_Click(object sender, RoutedEventArgs e)
        {
            stationPage = new(e);
            stationPage.StationEntityAddButton.Click += StationListPageButton_Click;
            Content = stationPage;
        }

        private void StationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StationToList station = (StationToList)stationListPage.StationListView.SelectedItem;
            if (station != null)
            {
                stationPage = new(station);
                stationPage.StationListGoBackButton.Click += StationListPageButton_Click;
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
                dronePage.DroneListGoBackButton.Click += DroneListPageButton_Click;
                Content = dronePage;
            }
        }
        #endregion

        #region Customers
        private void CustomerListPageButton_Click(object sender, RoutedEventArgs e)
        {
            //customerListPage = new();
            //customerListPage.CustomerListAddButton.Click += CustomerListAddButton_Click;
            //customerListPage.BackWindow.Click += BackWindow_Click;
            //customerListPage.CustomersListView.MouseDoubleClick += CustomerListView_MouseDoubleClick;
            //Content = customerListPage;
        }

        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList customer = (CustomerToList)customerListPage.CustomersListView.SelectedItem;
            if (customer != null)
            {
                //customerPage = new(customer);
                //customerPage.CustomerListGoBackButton.Click += StationListPageButton_Click;
                //customerPage.DronesListViewFromStation.MouseDoubleClick += DronesListViewFromStation_MouseDoubleClick;
                //Content = customerPage;
            }
        }

        private void CustomerListAddButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Parcels
        private void ParcelListPageButton_Click(object sender, RoutedEventArgs e)
        {
            parcelListPage = new();
            parcelListPage.ParcelDataGridAddButton.Click += ParcelDataGridAddButton_Click;
            parcelListPage.BackWindow.Click += BackWindow_Click;
            parcelListPage.ParcelDataGrid.MouseDoubleClick += ParcelDataGrid_MouseDoubleClick;
            Content = parcelListPage;
        }

        private void ParcelDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList parcel = (ParcelToList)parcelListPage.ParcelDataGrid.SelectedItem;
            if (parcel != null)
            {
                parcelPage = new(parcel.ID);
                parcelPage.ParcelDataGridGoBackButton.Click += ParcelListPageButton_Click;
                parcelPage.UpdateDeleteParcelButton.Click += ParcelListPageButton_Click;
                parcelPage.ParcelEntityAddButton.Click += ParcelListPageButton_Click;
                Content = parcelPage;
            }
        }

        private void ParcelDataGridAddButton_Click(object sender, RoutedEventArgs e)
        {
            parcelPage = new(e);
            parcelPage.ParcelEntityAddButton.Click += ParcelListPageButton_Click;
            Content = parcelPage;
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if (int.TryParse(txtUserID.Text, out id))
            {
                try
                {
                    CustomerBL customer = IBL.GetCustomer(id);
                }
                catch (InvalidIDException msg)
                {
                    //MessageBoxResult res = MessageBox.Show(msg.Message + msg.InnerException.Message, "Verification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //if (res == MessageBoxResult.No)
                    //    return;
                }
            }
            if(rbManager.IsChecked.Value)
            {
                LoginPanel.Visibility = Visibility.Collapsed;
                ManagerPanel.Visibility = Visibility.Visible;
            }
            else
            {

            }
        }
    }
}
