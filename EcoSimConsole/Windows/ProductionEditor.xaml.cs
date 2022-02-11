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
    /// Interaction logic for NewProduction.xaml
    /// </summary>
    public partial class ProductionEditor : Window
    {
        public ProductionEditorControl control;

        public ProductionEditor()
        {
            //InitializeComponent();
            //control = new ProductionEditorControl();
            //DataContext = control;
        }

        public ProductionEditor(Location location)
        {
            InitializeComponent();
            control = new ProductionEditorControl(location);
            DataContext = control;
        }

        public ProductionEditor(Production productionProcess, Location _location)
        {
            //InitializeComponent();
            //control = new ProductionEditorControl(productionProcess, _location);
            //DataContext = control;
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            control.Cancel();
        }

        private void SubmitClicked(object sender, RoutedEventArgs e)
        {
            //control.Save();
        }

        private void NewCommodityClicked(object sender, RoutedEventArgs e)
        {
            control.CreateNewCommodity();
        }

        private void AddRequirement(object sender, RoutedEventArgs e)
        {
            //control.AddRequirement();
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            control.Remove();
        }
    }
}