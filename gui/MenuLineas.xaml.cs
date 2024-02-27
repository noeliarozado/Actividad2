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
    public partial class MenuLineas : Window
    {
        public MenuLineas()
        {
            InitializeComponent();
        }
        private void btnAnadir_Click(object sender, RoutedEventArgs e)
        {
            AnadirLinea anadirLinea = new AnadirLinea();
            anadirLinea.Show();
            this.Close();
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            BorrarLinea borrarLinea = new BorrarLinea();
            borrarLinea.Show();
            this.Close();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            ModificarLinea modificarLinea = new ModificarLinea();
            modificarLinea.Show();
            this.Close();
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            ConsultarLineas consultarLinea = new ConsultarLineas();
            consultarLinea.Show();
            this.Close();
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            menuPrincipal.Show();
            this.Close();
        }
    }
}
