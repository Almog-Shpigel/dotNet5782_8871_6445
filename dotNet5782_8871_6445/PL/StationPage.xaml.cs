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

namespace PL
{
    /// <summary>
    /// Interaction logic for StationPage.xaml
    /// </summary>
    public partial class StationPage : Page
    {
        private ListViewItem item;
        private BlApi.IBL BLW;
        private BO.StationToList Station;
        private BO.StationBL StationBL;
        public StationPage(BlApi.IBL IBL, ListViewItem item)
        {
            InitializeComponent();
            BLW = IBL;
            this.item = item;
            Station = (BO.StationToList)item.DataContext;
            StationBL = IBL.GetStation(Station.ID);
            DataContext = StationBL;
            DronesListView.ItemsSource = StationBL.ChargingDrones;
        }

        private void UpdateNameButton_Click(object sender, RoutedEventArgs e)
        {
            BLW.UpdateStationName(Convert.ToInt32(IDBlock.Text), UpdateNameBlock.Text);
            DataContext = BLW.GetStation(Station.ID);
        }

        private void UpdateChargeSlotsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLW.UpdateStationSlots(Station.ID, Convert.ToInt32(UpdateChargeSlotsBlock.Text));
                DataContext = BLW.GetStation(Station.ID);
            }
            catch (Exception exp)
            {
                InvalidChargeSlotsBlock.Text = exp.Message;
                InvalidChargeSlotsBlock.Visibility = Visibility.Visible;
            }
            
        }
        private void EnterChargeSlotsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int Slots;
            if (!int.TryParse(UpdateChargeSlotsBlock.Text, out Slots))
            {
                InvalidChargeSlotsBlock.Visibility = Visibility.Visible;
                UpdateChargeSlotsBlock.Foreground = Brushes.Red;
                UpdateChargeSlotsButton.IsEnabled = false;
            }
            else
            {
                UpdateChargeSlotsButton.IsEnabled = true;
                InvalidChargeSlotsBlock.Visibility = Visibility.Collapsed;
                UpdateChargeSlotsBlock.Foreground = Brushes.Black;


            }
        }

        private void UpdateDeleteStationButton_Click(object sender, RoutedEventArgs e)
        {

        }

      
    }
}
