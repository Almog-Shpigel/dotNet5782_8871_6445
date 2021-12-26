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

        public MainWindow()
        {
            IBL = BlFactory.GetBl();
            InitializeComponent();
        }

        private void DroneListPageButton_Click(object sender, RoutedEventArgs e)
        {
            DroneListPage droneList = new DroneListPage();
            droneList.AddNewDrone.Click += AddNewDrone_Click;
            this.Content = droneList;
        }

        private void AddNewDrone_Click(object sender, RoutedEventArgs e)
        {
            DronePage dronePage = new DronePage(e);
            dronePage.AddNewDroneButton.Click += AddNewDroneButton_Click;
            this.Content = dronePage;
        }

        private void AddNewDroneButton_Click(object sender, RoutedEventArgs e)
        {
            DronePage dronePage = new DronePage(e);
            WeightCategories weight = (WeightCategories)Enum.Parse(typeof(WeightCategories), (string)dronePage.WeightSelector.SelectedItem);
            DroneBL drone = new(Convert.ToInt32(dronePage.EnterDroneIDBox.Text), dronePage.EnterModelNameBox.Text, weight);
            try
            {
                IBL.AddNewDrone(drone, Convert.ToInt32(dronePage.StationSelector.Text));
                dronePage.EnterDroneIDBox.IsEnabled = false;
                dronePage.EnterModelNameBox.IsEnabled = false;

                dronePage.StationSelector.IsEnabled = false;
                dronePage.WeightSelector.IsEnabled = false;
                dronePage.AddNewDroneButton.IsEnabled = false;
                //   NavigationService.GoBack();
            }
            catch (Exception ex)
            {

            }
            //catch (InvalidIDException exp)
            //{
            //    InvalidDroneIDBlock.Text = exp.Message;
            //    InvalidDroneIDBlock.Visibility = Visibility.Visible;
            //    EnterDroneIDBox.Foreground = Brushes.Red;
            //    AddNewDroneButton.IsEnabled = false;
            //}
            //catch (DroneExistExceptionBL exp)
            //{
            //    InvalidDroneIDBlock.Text = exp.Message;
            //    InvalidDroneIDBlock.Visibility = Visibility.Visible;
            //    EnterDroneIDBox.Foreground = Brushes.Red;
            //    AddNewDroneButton.IsEnabled = false;
            //}
            //catch (StationExistExceptionBL exp)
            //{
            //    InvalidStationIDBlock.Text = exp.Message;
            //    InvalidStationIDBlock.Visibility = Visibility.Visible;
            //    AddNewDroneButton.IsEnabled = false;
            //}
            //catch (InvalidSlotsException exp)
            //{
            //    InvalidStationIDBlock.Text = exp.Message;
            //    InvalidStationIDBlock.Visibility = Visibility.Visible;
            //    AddNewDroneButton.IsEnabled = false;
            //}
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
