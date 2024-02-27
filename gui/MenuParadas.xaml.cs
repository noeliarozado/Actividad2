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
    public partial class MenuParadas : Window
    {
        public MenuParadas()
        {
            InitializeComponent();
        }

        private void btnAnadir_Click(object sender, RoutedEventArgs e)
        {
            AnadirParada anadirParada = new AnadirParada();
            anadirParada.Show();
            this.Close();
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            BorrarParada borrarParada = new BorrarParada();
            borrarParada.Show();
            this.Close();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            ModificarParada modificarParada = new ModificarParada();
            modificarParada.Show();
            this.Close();
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            ConsultarParadas consultarParada = new ConsultarParadas();
            consultarParada.Show();
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
