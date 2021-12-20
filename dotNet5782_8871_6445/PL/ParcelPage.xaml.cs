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
        private BlApi.IBL BLW;
        private Frame MainFrame;
        public ParcelPage(BlApi.IBL IBL, RoutedEventArgs e, Frame Main)
        {
            InitializeComponent();
            BLW = IBL;
            MainFrame = Main;
        }
        public ParcelPage(BlApi.IBL BLW, ListViewItem item)
        {
            InitializeComponent();
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
