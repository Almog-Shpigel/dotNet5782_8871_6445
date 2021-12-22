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
    /// Interaction logic for ParcelPage.xaml
    /// </summary>
    public partial class ParcelPage : Page
    {
        private ListViewItem item;
        private BlApi.IBL BLW;
        private Frame MainFrame;
        private BO.ParcelToList Parcel;
        public ParcelPage(BlApi.IBL IBL, RoutedEventArgs e, Frame Main)
        {
            InitializeComponent();
            BLW = IBL;
            MainFrame = Main;
           
        }
        public ParcelPage(BlApi.IBL IBL, ListViewItem item, Frame Main)
        {
            InitializeComponent();
            BLW = IBL;
            this.item = item;
            MainFrame = Main;
            Parcel = (BO.ParcelToList)item.DataContext;
            DataContext = BLW.GetParcel(Parcel.ID);
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
    }
}
