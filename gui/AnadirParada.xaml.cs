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
    public partial class AnadirParada : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        public string Error { get { return string.Empty; } }

        private int errores = 0;

        private LogicaBus logicaBus = new LogicaBus();

        private string ultimoMunicipioAnadido;
        private TimeSpan ultimoIntervaloAnadido;

        private List<string> municipiosAnadidos = new List<string>();
        private List<ParadaConIntervalo> paradasConIntervalos = new List<ParadaConIntervalo>();

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
                btnAnadir.IsEnabled = true;
            else
                btnAnadir.IsEnabled = false;
        }

        public AnadirParada()
        {
            InitializeComponent();

            Intervalo = ":";

            DataContext = this;

            List<int> lineas = LeerCSVLineas("..\\..\\..\\data\\lineas.csv");
            lineas.Sort();
            ComboLineas = lineas;

            Municipios = LeerCSVMunicipios("..\\..\\..\\data\\municipios.csv");

            this.ResizeMode = ResizeMode.NoResize;
        }

        /// <summary>
        /// Lee las líneas de bus del archivo CSV
        /// </summary>
        /// <param name="ruta">Ruta del archivo CSV</param>
        /// <returns>Lista de líneas</returns>
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
        /// <param name="ruta">Ruta del archivo CSV</param>
        /// <returns>Lista de municipios</returns>
        private List<string> LeerCSVMunicipios(string ruta)
        {
            List<string> municipios = new List<string>();

            string[] lineas = File.ReadAllLines(ruta);

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

        private void btnAnadir_Click(object sender, RoutedEventArgs e)
        {
            string municipio = comboMunicipio.SelectedItem as string;
            TimeSpan intervaloHoraSalida = TimeSpan.ParseExact(textboxIntervaloHoraSalida.Text,
                "hh\\:mm", null);

            if (comboNumeroLinea.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione un número de línea.");
                return;
            }

            if (!int.TryParse(comboNumeroLinea.SelectedItem.ToString(), out int numeroLinea))
            {
                MessageBox.Show("Error. Formato inválido para el número de línea.");
                return;
            }

            if (municipiosAnadidos.Contains(municipio))
            {
                MessageBox.Show("Este municipio ya ha sido seleccionado. Por favor, escoja uno diferente");
                return;
            }

            paradasConIntervalos.Add(new ParadaConIntervalo
            {
                Municipio = municipio,
                IntervaloDesdeHoraSalida = intervaloHoraSalida
            });

            if (paradasConIntervalos.Count > 1)
            {
                ParadaConIntervalo destinationStop = paradasConIntervalos
                .OrderByDescending(stop => stop.IntervaloDesdeHoraSalida).First();

                if (destinationStop.Municipio != municipio)
                {
                    MessageBox.Show("Este intervalo tiene que ser mayor que el de la anterior parada.");
                    return;
                }
            }
            logicaBus.AnadirParadaALista(new Parada(numeroLinea, municipio, intervaloHoraSalida));
            MessageBox.Show("¡Parada añadida con éxito!");
        }

        private void btnAnadirNuevaParada_Click(object sender, RoutedEventArgs e)
        {
            ultimoMunicipioAnadido = comboMunicipio.Text;
            ultimoIntervaloAnadido = TimeSpan.ParseExact(textboxIntervaloHoraSalida.Text, "hh\\:mm", null);

            municipiosAnadidos.Add(ultimoMunicipioAnadido);

            paradasConIntervalos.Add(new ParadaConIntervalo
            {
                Municipio = ultimoMunicipioAnadido,
                IntervaloDesdeHoraSalida = ultimoIntervaloAnadido
            });

            comboMunicipio.Text = string.Empty;
            textboxIntervaloHoraSalida.Text = ":";
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
