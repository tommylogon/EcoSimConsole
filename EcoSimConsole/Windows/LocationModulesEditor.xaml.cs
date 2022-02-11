using EcoSimConsole.Control;
using EcoSimConsole.Data;
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
    /// Interaction logic for LocationModulesEditor.xaml
    /// </summary>
    public partial class LocationModulesEditor : Window
    {
        public LocationModuleEditorControl control;
        public LocationModulesEditor(Location location)
        {
            InitializeComponent();
            control = new LocationModuleEditorControl(location);
            DataContext = control;
        }

        private void SelectedModuleChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems[0] != null && e.AddedItems[0] is LocationModule lm)
            {
                control.SelectedModule = lm;
            }
        }
    }
}
