using Actividad2.dto;
using Actividad2.logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    public partial class ModificarLinea : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        public string Error { get { return string.Empty; } }

        private int errores = 0;

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

        private string numeroLinea;

        public string NumeroLinea
        {
            get { return numeroLinea; }
            set
            {
                if (numeroLinea != value)
                {
                    numeroLinea = value;
                    OnPropertyChanged(nameof(NumeroLinea));
                }
            }
        }

        private string horaSalida;

        public string HoraSalida
        {
            get { return horaSalida; }
            set
            {
                if (horaSalida != value)
                {
                    horaSalida = value;
                    OnPropertyChanged(nameof(HoraSalida));
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

                if (columnName == "NumeroLinea")
                {
                    try
                    {
                        int.Parse(numeroLinea);
                    }
                    catch (Exception)
                    {

                        result = "Por favor, introduzca el número de línea";
                    }
                }

                if (columnName == "HoraSalida")
                {
                    try
                    {
                        DateTime.Parse(horaSalida);
                    }
                    catch (Exception)
                    {
                        result = "Por favor, introduzca la hora inicial de salida.";
                    }
                }

                if (columnName == "Intervalo")
                {
                    try
                    {
                        TimeSpan.Parse(intervalo);
                    }
                    catch (Exception)
                    {
                        result = "Por favor, introduzca el intervalo entre buses.";
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

        public ModificarLinea()
        {
            InitializeComponent();

            ComboLineas = logicaBus.GetListaLineas().Select(linea => linea.NumeroLinea)
            .OrderBy(number => number).ToList();

            DataContext = this;

            PrecargarInput();

            //Municipios = LeerCSVMunicipios("..\\..\\..\\data\\municipios.csv");
            string municipiosFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                "data", "municipios.csv");
            Municipios = LeerCSVMunicipios(municipiosFilePath);
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

        /// <summary>
        /// Carga la información de la línea de autobús
        /// </summary>
        private void PrecargarInput()
        {
            if (comboNumeroLinea.SelectedItem != null)
            {
                int numeroLinea = (int)comboNumeroLinea.SelectedItem;

                List<Linea> listaLineas = logicaBus.GetListaLineas();

                Linea lineaSeleccionada = listaLineas
                .FirstOrDefault(linea => linea.NumeroLinea == numeroLinea);

                if (lineaSeleccionada != null)
                {
                    NumeroLinea = lineaSeleccionada.NumeroLinea.ToString();
                    comboOrigen.SelectedItem = lineaSeleccionada.MunicipioOrigen.ToString();
                    comboDestino.SelectedItem = lineaSeleccionada.MunicipioDestino;
                    HoraSalida = lineaSeleccionada.HoraInicialSalida.ToString("HH:mm");
                    Intervalo = lineaSeleccionada.IntervaloEntreBuses.ToString("hh\\:mm");
                }
            }
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            int numeroLinea = int.Parse(textboxNumeroLinea.Text);
            string municipioOrigen = comboOrigen.SelectedItem as string;
            string municipioDestino = comboDestino.SelectedItem as string;
            DateTime horaInicialSalida = DateTime.ParseExact(textboxHoraSalida.Text, "HH:mm",
                CultureInfo.InvariantCulture);
            TimeSpan intervaloEntreBuses = TimeSpan.Parse(textboxIntervalo.Text);

            if (municipioOrigen == municipioDestino)
            {
                MessageBox.Show("El municipio de origen y el municipio de destino no pueden ser iguales.");
                return;
            }

            logicaBus.ActualizarLinea(new Linea(numeroLinea, municipioOrigen, municipioDestino,
                horaInicialSalida, intervaloEntreBuses));
            MessageBox.Show("¡Línea modificada con éxito!");
        }

        private void LineNumberCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PrecargarInput();
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
