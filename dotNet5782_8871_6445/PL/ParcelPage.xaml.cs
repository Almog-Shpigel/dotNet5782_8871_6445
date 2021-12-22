using System.Windows;
using System.Windows.Controls;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelPage.xaml
    /// </summary>
    public partial class ParcelPage : Page
    {
        private ListViewItem item;
        private BlApi.IBL BLW;
        private BO.ParcelToList Parcel;
        private BO.ParcelBL ParcelBL;
        Frame Frame;

        public ParcelPage(BlApi.IBL IBL, RoutedEventArgs e, Frame frame)
        {
            InitializeComponent(); // Add drone ctor
            BLW = IBL;
            Frame = frame;
            UpdateLayout();
        }

        public ParcelPage(BlApi.IBL IBL, ListViewItem item, Frame frame) // Update parcel ctor
        {
            InitializeComponent();
            BLW = IBL;
            this.item = item;
            Frame = frame;
            Parcel = (BO.ParcelToList)item.DataContext;
            ParcelBL = BLW.GetParcel(Parcel.ID);
            DataContext = ParcelBL;
            UpdateLayout();
        }

        private void EnterSenderIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void EnterTargetIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void UpdateDeleteParcelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddNewParcelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UpdateParcelCollectedButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateParcelDeliveredButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
