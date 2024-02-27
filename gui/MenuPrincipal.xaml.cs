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

namespace Actividad2.gui
{
    public partial class MenuPrincipal : Window
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }
        private void btnLineas_Click(object sender, RoutedEventArgs e)
        {
            MenuLineas menuLineas = new MenuLineas();
            menuLineas.Show();
            this.Close();
        }

        private void btnParadas_Click(object sender, RoutedEventArgs e)
        {
            MenuParadas menuParadas = new MenuParadas();
            menuParadas.Show();
            this.Close();
        }
    }
}
