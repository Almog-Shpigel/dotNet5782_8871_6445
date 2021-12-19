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
            this.Content = new DroneListPage(IBL);
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            new MapWindow(IBL).Show();
            Close();
        }

        private void StationListPageButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Content = new StationListPage(IBL);
        }

        private void CustomerListPageButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Content = new CustomerListPage(IBL);
        }
    }
}
