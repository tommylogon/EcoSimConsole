using EcoSimConsole.Control;
using EcoSimConsole.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace EcoSimConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainControl();
            
        }

        private void ViewLocations(object sender, RoutedEventArgs e)
        {
            MainControl.main.ViewLocations(sender, e);
        }

        private void ViewCitizens(object sender, RoutedEventArgs e)
        {
            MainControl.main.ViewCitizens(sender, e);
        }

        private void NewProductionClicked(object sender, RoutedEventArgs e)
        {
            //if (!MainControl.main.windowActive)
            MainControl.main.OpenProductionEditor();
        }

        private void NewLocationClicked(object sender, RoutedEventArgs e)
        {
            MainControl.main.CreateNewLocation();
        }

        private void ResetPricesClicked(object sender, RoutedEventArgs e)
        {
            MainControl.main.ResetPrices();
        }

        

        private void ViewModulesClicked(object sender, RoutedEventArgs e)
        {

            MainControl.main.OpenModuleViewer();
        }

        

        private void StarMap_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(e.NewValue is Location l)
            {
                MainControl.main.SelectedLocation = l;
            }
            
        }
    }
}