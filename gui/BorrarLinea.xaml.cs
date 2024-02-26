using Actividad2.dto;
using Actividad2.logic;
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
using System.Windows.Shapes;

namespace Actividad2.gui
{
    public partial class BorrarLinea : Window, INotifyPropertyChanged
    {
        private LogicaBus logicaBus = new LogicaBus();

        private List<int> comboLineas;

        public List<int> ComboLineas
        {
            get { return comboLineas; }
            set
            {
                if (comboLineas != value)
                {
                    comboLineas = value;
                    OnPropertyChanged(nameof(ComboLineas));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BorrarLinea()
        {
            InitializeComponent();

            ComboLineas = logicaBus.GetListaLineas().Select(linea => linea.NumeroLinea)
            .OrderBy(num => num).ToList();

            DataContext = this;
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            int? numeroLineaSeleccionado = comboBorrar.SelectedItem as int?;

            if (numeroLineaSeleccionado != null)
            {
                Linea lineaSeleccionada = logicaBus
                .GetListaLineas().FirstOrDefault(l => l.NumeroLinea == numeroLineaSeleccionado);

                if (lineaSeleccionada != null)
                {
                    logicaBus.BorrarLinea(lineaSeleccionada);
                    MessageBox.Show("¡Línea borrada con éxito!");

                    // Actualiza el ComboBox después del borrado
                    List<Linea> lineasActualizadas = logicaBus.GetListaLineas();
                    comboBorrar.ItemsSource = lineasActualizadas;
                }
            }
        }
        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            MenuLineas menuLineas = new MenuLineas();
            menuLineas.Show();
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
