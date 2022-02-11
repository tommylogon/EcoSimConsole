using EcoSimConsole.Control;
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

namespace EcoSimConsole.Windows
{
    /// <summary>
    /// Interaction logic for NewCommodity.xaml
    /// </summary>
    public partial class CommodityEditor : Window
    {
        private CommodityEditorControl controll = new CommodityEditorControl();

        public CommodityEditor()
        {
            InitializeComponent();
            DataContext = controll;
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            controll.Save();
        }

        private void SelectioNChanged(object sender, SelectionChangedEventArgs e)
        {
            controll.NotifyAll(e);
        }
    }
}