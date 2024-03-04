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
    /// <summary>
    /// Interaction logic for BorrarParada.xaml
    /// </summary>
    public partial class BorrarParada : Window, INotifyPropertyChanged
    {
        private LogicaBus logicaBus = new LogicaBus();

        private List<Parada> paradas;

        public List<Parada> Paradas
        {
            get { return paradas; }
            set
            {
                paradas = value;
                OnPropertyChanged(nameof(Paradas));
            }
        }

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

        public BorrarParada()
        {
            InitializeComponent();

            CargarParadas();

            ComboLineas = logicaBus.GetListaParadas()
            .Select(linea => linea.NumeroLinea).Distinct().OrderBy(num => num).ToList();

            DataContext = this;

            btnBorrar.IsEnabled = false;
            btnBorrarItinerario.IsEnabled = false;
        }

        private void dgParadas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgParadas.SelectedItem != null)
            {
                btnBorrar.IsEnabled = true;
            }
            else
            {
                btnBorrar.IsEnabled = false;
            }
        }

        private void comboBorrar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBorrar.SelectedItem != null)
            {
                btnBorrarItinerario.IsEnabled = true;
            }
            else
            {
                btnBorrarItinerario.IsEnabled = false;
            }
        }

        /// <summary>
        /// Carga las paradas y las ordena, primero por número de línea y después por intervalo 
        /// desde la hora de salida
        /// </summary>
        private void CargarParadas()
        {
            List<Parada> paradas = logicaBus.GetListaParadas();

            Paradas = paradas.OrderBy(stop => stop.NumeroLinea)
                          .ThenBy(stop => stop.IntervaloDesdeHoraSalida)
                          .ToList();
        }

        /// <summary>
        /// Actualiza los números de línea del ComboBox
        /// </summary>
        private void ActualizarComboLineas()
        {
            List<Parada> stops = logicaBus.GetListaParadas()
                                        .GroupBy(stop => stop.NumeroLinea)
                                        .Select(group => group.First())
                                        .ToList();

            ComboLineas = stops.Select(stop => stop.NumeroLinea).OrderBy(number => number).ToList();
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dgParadas.SelectedItem != null)
            {
                Parada paradaSeleccionada = (Parada)dgParadas.SelectedItem;

                logicaBus.BorrarParada(paradaSeleccionada);

                MessageBox.Show("¡Parada borrada con éxito!");

                CargarParadas();

                OnPropertyChanged(nameof(Paradas));
            }
            else
            {
                MessageBox.Show("Por favor, seleccione la parada a borrar.");
            }
        }

        private void btnBorrarItinerario_Click(object sender, RoutedEventArgs e)
        {
            int? numeroLineaSeleccionado = comboBorrar.SelectedItem as int?;

            if (numeroLineaSeleccionado != null)
            {
                logicaBus.BorrarTodasParadas((int)numeroLineaSeleccionado);

                MessageBox.Show("¡Itinerario borrado con éxito!");

                ActualizarComboLineas();

                CargarParadas();
            }
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            MenuParadas menuParadas = new MenuParadas();
            menuParadas.Show();
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
