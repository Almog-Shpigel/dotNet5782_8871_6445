using Microsoft.Maps.MapControl.WPF;
using System.Windows;
//using Windows.UI.Xaml.Controls.Maps;

namespace PL
{
    /// <summary>
    /// Interaction logic for MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        public MapWindow(BlApi.IBL IBL)
        {
            InitializeComponent();
            Pushpin pin;
            foreach (var item in IBL.GetAllDroneLocations())
            {
                pin = new();
                pin.Location = new(item.Longitude, item.Latitude);
                myMap.Children.Add(pin);
            }
            foreach (var item in IBL.GetAllStationsLocations())
            {
                pin = new();
                pin.Location = new(item.Longitude, item.Latitude);
                myMap.Children.Add(pin);
            }
        }
    }
}
