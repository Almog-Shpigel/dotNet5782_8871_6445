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
        }
    }
}
