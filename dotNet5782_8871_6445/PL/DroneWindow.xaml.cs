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
using System.Windows.Shapes;
using IDAL.DO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private ListViewItem item;
        private IBL.BL BLW;
        private IBL.BO.DroneToList Drone;
        public DroneWindow(IBL.BL IBL)
        {
            InitializeComponent();
        }

        public DroneWindow(IBL.BL IBL, ListViewItem item) : this(IBL)
        {
            this.item = item;
            BLW = IBL;
            Drone =(IBL.BO.DroneToList)item.DataContext;
            IBL.BO.DroneBL droneBL = BLW.DisplayDrone(Drone.ID);
            IDBlock.Text =droneBL.ID.ToString();
            ModelBlock.Text = droneBL.Model;
            WeightBlock.Text = droneBL.MaxWeight.ToString();
            BatteryBlock.Text = droneBL.BatteryStatus.ToString();
            StatusBlock.Text = droneBL.Status.ToString();
            ParcelBlock.Text = droneBL.Parcel.ID.ToString();
            LocationBlock.Text = droneBL.CurrentLocation.ToString();

        }

        private void UpdateNameButton_Click(object sender, RoutedEventArgs e)
        {
            BLW.UpdateDroneName(Convert.ToInt32(IDBlock.Text),UpdateNameBlock.Text);
            new DroneListWindow(BLW).Show();
            // Needs to change the button to inform the user the update accured
        }
    }
}
