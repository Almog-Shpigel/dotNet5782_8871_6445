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
        private Frame MainFrame;
        private BlApi.IBL BLW;
        private BO.StationToList Station;
        private BO.StationBL StationBL;
        public StationPage(BlApi.IBL IBL, ListViewItem item,Frame frame)
        {
            InitializeComponent();
            BLW = IBL;
            this.item = item;
            MainFrame = frame;
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
                BLW.UpdateStationSlots(Station.ID, Convert.ToInt32(UpdateChargeSlotsBlock.Text),StationBL.ChargingDrones.Count());
                DataContext = BLW.GetStation(Station.ID);
            }
            catch (Exception exp)
            {
                InvalidInputBlock.Text = exp.Message;
                InvalidInputBlock.Visibility = Visibility.Visible;
            }
            
        }
        private void EnterChargeSlotsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int Slots;
            if (!int.TryParse(UpdateChargeSlotsBlock.Text, out Slots))
            {
                InvalidInputBlock.Visibility = Visibility.Visible;
                UpdateChargeSlotsBlock.Foreground = Brushes.Red;
                UpdateChargeSlotsButton.IsEnabled = false;
            }
            else
            {
                UpdateChargeSlotsButton.IsEnabled = true;
                InvalidInputBlock.Visibility = Visibility.Collapsed;
                UpdateChargeSlotsBlock.Foreground = Brushes.Black;


            }
        }

        private void UpdateDeleteStationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLW.DeleteStation(StationBL.ID);
                MainFrame.Content = new StationListPage(BLW, MainFrame);
            }
            catch (Exception exp)
            {

                InvalidInputBlock.Text = exp.Message;
                InvalidInputBlock.Visibility = Visibility.Visible;
            }
        }

      
    }
}
