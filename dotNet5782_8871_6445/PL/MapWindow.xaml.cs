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
            foreach (var item in IBL.GetAllDrones())
            {
                AddPushpin(new(item.CurrentLocation.Longitude, item.CurrentLocation.Latitude), item.ID);
            }
            foreach (var item in IBL.GatAllStations())
            {
                //AddPushpin(new(item.lo Longitude, item.Latitude));
            }

        }

        public void AddPushpin(Microsoft.Maps.MapControl.WPF.Location latlong, int ID)
        {
            Pushpin pin = new Pushpin();
            pin.Location = latlong;
            pin.MouseDown += PinClicked;
            myMap.Children.Add(pin);
        }

        private void PinClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Pushpin pin = sender as Pushpin;
            BO.Location location = new(pin.Location.Latitude, pin.Location.Longitude);
            //StationBL station = BLW.GetStationByLocation(location);
            //var item = sender as StationBL;
            //if (item != null)
            //{
            //    new DroneWindow(BLW, e).Show();
            //}
            //pin.Content = "Hover over me.";
            var tt = new ToolTip();
            tt.Content = "Station info";
            pin.ToolTip = tt;
            //myMap.Children.Add(pin);
            //Add logic on what to do when the pushpin is clicked
        }

        private void CloseInfobox_Click(object sender, RoutedEventArgs e)
        {
            //Infobox.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
