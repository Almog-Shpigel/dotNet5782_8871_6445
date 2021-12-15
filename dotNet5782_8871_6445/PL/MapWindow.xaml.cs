using BO;
using Microsoft.Maps.MapControl.WPF;
using System.Windows;
using System.Windows.Controls;
//using Windows.UI.Xaml.Controls.Maps;

namespace PL
{
    /// <summary>
    /// Interaction logic for MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        private BlApi.IBL BLW;
        public MapWindow(BlApi.IBL IBL)
        {
            InitializeComponent();
            Pushpin pin = new();
            BLW = IBL;
            foreach (var item in BLW.GetAllDrones())
            {
                AddPushpin(new(item.CurrentLocation.Longitude, item.CurrentLocation.Latitude), item.ID);
            }
            foreach (var item in BLW.GatAllStationsDO())
            {
                AddPushpin(new(item.Longitude, item.Latitude), item.ID);
            }
        }

        public void AddPushpin(Microsoft.Maps.MapControl.WPF.Location latlong, int ID)
        {
            Pushpin pin = new();
            ToolTip tt = new();
            StationBL station;
            DroneBL drone;
            pin.Location = latlong;
            //pin.MouseDown += PinClicked;
            pin.Tag = ID;
            try
            {
                station = BLW.GetStation((int)pin.Tag);
                tt.Content = station.ToString();
            }
            catch (System.Exception)
            {
            }
            try
            {
                drone = BLW.GetDrone((int)pin.Tag);
                tt.Content = drone.ToString();
            }
            catch (System.Exception)
            {
            }
            pin.ToolTip = tt;
            myMap.Children.Add(pin);
        }

        private void PinClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Pushpin pin = sender as Pushpin;
            StationBL station = BLW.GetStation((int)pin.Tag);
            ToolTip tt = new();
            tt.Content = station.ToString();
            pin.ToolTip = tt;
        }

        private void CloseInfobox_Click(object sender, RoutedEventArgs e)
        {
            //Infobox.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
