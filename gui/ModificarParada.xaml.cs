using Actividad2.dto;
using Actividad2.logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// Interaction logic for ModificarParada.xaml
    /// </summary>
    public partial class ModificarParada : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        public string Error { get { return string.Empty; } }

        private int errores = 0;

        private LogicaBus logicaBus = new LogicaBus();

        private List<Parada> paradas;

        public List<Parada> Paradas
        {
            get { return paradas; }
            set { paradas = value; OnPropertyChanged(nameof(paradas)); }
        }

        private List<string> municipios;

        public List<string> Municipios
        {
            get { return municipios; }
            set
            {
                if (municipios != value)
                {
                    municipios = value;
                    OnPropertyChanged(nameof(Municipios));
                }
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

        private string intervalo;

        public string Intervalo
        {
            get { return intervalo; }
            set
            {
                if (intervalo != value)
                {
                    intervalo = value;
                    OnPropertyChanged(nameof(Intervalo));
                }
            }
        }

        /// <summary>
        /// Indexador para la validación de datos
        /// </summary>
        /// <param name="columnName">Nombre a validar</param>
        /// <returns>Un mensaje de error si la validación detecta un error</returns>
        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;

                if (columnName == "Intervalo")
                {
                    try
                    {
                        TimeSpan.Parse(intervalo);
                    }
                    catch (Exception)
                    {
                        result = "Por favor, introduzca el intervalo desde la hora de salida.";
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Maneja los errores de validación
        /// </summary>
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                errores++;
            else
                errores--;
            if (errores == 0)
                btnModificar.IsEnabled = true;
            else
                btnModificar.IsEnabled = false;
        }

        public ModificarParada()
        {
            InitializeComponent();

            DataContext = this;

            List<int> lineas = LeerCSVLineas("..\\..\\..\\data\\lineas.csv");
            lineas.Sort();
            ComboLineas = lineas;

            CargarParadasOrdenadas();
        }

        /// <summary>
        /// Carga las paradas y las ordena, primero por número de línea y después por intervalo 
        /// desde la hora de salida
        /// </summary>
        private void CargarParadasOrdenadas()
        {
            Paradas = logicaBus.GetListaParadas();

            Paradas = Paradas.OrderBy(stop => stop.NumeroLinea)
                .ThenBy(stop => stop.IntervaloDesdeHoraSalida)
                .ToList();

            Municipios = LeerCSVMunicipios("..\\..\\..\\data\\municipios.csv");
        }

        private List<int> LeerCSVLineas(string ruta)
        {
            List<int> lineas = new List<int>();

            string[] archivoLineas = File.ReadAllLines(ruta);

            foreach (string linea in archivoLineas)
            {
                string[] partes = linea.Split(',');
                if (partes.Length >= 1)
                {
                    int numeroLinea;
                    if (int.TryParse(partes[0], out numeroLinea))
                    {
                        lineas.Add(numeroLinea);
                    }
                }
            }
            return lineas;
        }

        /// <summary>
        /// Lee los municipios del archivo CSV
        /// </summary>
        /// <param name="filePath">Ruta del archivo CSV</param>
        /// <returns>Lista de municipios</returns>
        private List<string> LeerCSVMunicipios(string filePath)
        {
            List<string> municipios = new List<string>();

            string[] lineas = File.ReadAllLines(filePath);

            foreach (string linea in lineas)
            {
                string[] partes = linea.Split(',');
                if (partes.Length >= 1)
                {
                    municipios.Add(partes[0]);
                }
            }

            return municipios;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgParadas.SelectedIndex != -1)
            {
                Parada paradaSeleccionada = Paradas[dgParadas.SelectedIndex];
                Parada paradaOriginal = new Parada(paradaSeleccionada.NumeroLinea, paradaSeleccionada.Municipio, 
                    paradaSeleccionada.IntervaloDesdeHoraSalida);

                if (!string.IsNullOrEmpty(comboNumeroLinea.Text)
                && !string.IsNullOrEmpty(textboxIntervalo.Text)
                    && !string.IsNullOrEmpty(comboMunicipio.Text))
                {
                    int numeroLinea = int.Parse(comboNumeroLinea.Text);
                    string municipio = comboMunicipio.Text;
                    TimeSpan intervaloDesdeSalida;

                    if (TimeSpan.TryParseExact(textboxIntervalo.Text, "hh\\:mm", null, out intervaloDesdeSalida))
                    {
                        if (paradaExiste(numeroLinea, municipio))
                        {
                            MessageBox.Show("Error. La parada ya existe.");
                        }
                        else
                        {
                            paradaSeleccionada.NumeroLinea = numeroLinea;
                            paradaSeleccionada.Municipio = municipio;
                            paradaSeleccionada.IntervaloDesdeHoraSalida = intervaloDesdeSalida;

                            logicaBus.ActualizarParada(paradaSeleccionada, paradaOriginal);
                            MessageBox.Show("¡Parada modificada con éxito!");

                            CargarParadasOrdenadas();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Por favor, introduzca un intervalo desde la hora de salida válido.");
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, cumplimente todos los campos.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione la parada a modificar.");
            }
        }

        private bool paradaExiste(int numeroLinea,string municipio)
        {
            foreach (Parada parada in Paradas)
            {
                if (parada.NumeroLinea == numeroLinea &&
                   parada.Municipio == municipio)
                {
                    return true;
                }
            }
            return false;
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

        private void dgParadas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgParadas.SelectedItem != null)
            {
                if (dgParadas.SelectedItem != null)
                {
                    Parada paradaSeleccionada = (Parada)dgParadas.SelectedItem;

                    if (paradaSeleccionada != null)
                    {
                        if (paradaSeleccionada.NumeroLinea != null)
                        {
                            comboNumeroLinea.Text = paradaSeleccionada.NumeroLinea.ToString();
                        }

                        if (!string.IsNullOrEmpty(paradaSeleccionada.Municipio))
                        {
                            comboMunicipio.Text = paradaSeleccionada.Municipio;
                        }

                        if (paradaSeleccionada.IntervaloDesdeHoraSalida != null)
                        {
                            string intervaloModificado = paradaSeleccionada.IntervaloDesdeHoraSalida
                                .ToString().Remove(paradaSeleccionada.IntervaloDesdeHoraSalida.ToString()
                                .Length - 3);

                            textboxIntervalo.Text = intervaloModificado;
                        }
                    }
                }
            }
        }
    }
}
