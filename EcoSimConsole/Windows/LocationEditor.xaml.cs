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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EcoSimConsole.Windows
{
    /// <summary>
    /// Interaction logic for LocationEditor.xaml
    /// </summary>
    public partial class LocationEditor : Window
    {
        private LocationEditorControll controll = new LocationEditorControll();

        public LocationEditor()
        {
            InitializeComponent();
            DataContext = controll;
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            controll.Save();
            Close();
        }

        private void AddProductionClicked(object sender, RoutedEventArgs e)
        {
            controll.AddProduction();
        }

        private void SetPositionClicked(object sender, RoutedEventArgs e)
        {
            controll.SetPosition();
        }

        private void TypeChanged(object sender, SelectionChangedEventArgs e)
        {
            controll.ChangeContext();
        }

        private void DisplayChanged(object sender, SelectionChangedEventArgs e)
        {
            controll.ChangeAvailableFor();
        }

        private void SelectedLocationChanged(object sender, SelectionChangedEventArgs e)
        {
            controll.update = true;
            controll.SelectedType = controll.DisplayType;
            controll.NotifyAll();
        }
    }
}