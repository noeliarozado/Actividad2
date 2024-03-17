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
    public partial class AnadirLinea : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        public const string RUTA_FICHERO_CSV_MUNICIPIOS = "data\\municipios.csv";


        public string Error { get { return string.Empty; } }

        private int errores = 0;

        private LogicaBus logicaBus = new LogicaBus();

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

                if (columnName == "HoraSalida")
                {
                    try
                    {
                        DateTime.Parse(horaSalida);
                    }
                    catch (Exception)
                    {
                        result = "Por favor, introduzca la fecha.";
                    }
                }

                if (columnName == "NumeroLinea")
                {
                    try
                    {
                        int.Parse(numeroLinea);
                    }
                    catch (Exception)
                    {

                        result = "Por fvor, introduzca el número de línea.";
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

        public AnadirLinea()
        {
            InitializeComponent();

            HoraSalida = ":";
            Intervalo = ":";

            DataContext = this;

            //Municipios = LeerCSVMunicipios("..\\..\\..\\data\\municipios.csv");
            Municipios = LeerCSVMunicipios(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RUTA_FICHERO_CSV_MUNICIPIOS));
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

            foreach (string line in lineas)
            {
                string[] partes = line.Split(',');
                if (partes.Length >= 1)
                {
                    municipios.Add(partes[0]);
                }
            }
            return municipios;
        }

        private void btnAnadir_Click(object sender, RoutedEventArgs e)
        {
            int numeroLinea = int.Parse(textboxNumeroLinea.Text);
            string municipioOrigen = comboOrigen.SelectedItem as string;
            string municipioDestino = comboDestino.SelectedItem as string;
            DateTime horaInicialSalida = DateTime.ParseExact(textboxHoraInicialSalida.Text, "HH:mm", null);
            TimeSpan intervaloEntreBuses = TimeSpan.ParseExact(textboxIntervaloBuses.Text, @"hh\:mm", null);

            if (municipioOrigen == municipioDestino)
            {
                MessageBox.Show("El municipio de origen y el municipio de destino no pueden ser iguales.");
                return;
            }

            logicaBus.AnadirLineaALista(new Linea(numeroLinea, municipioOrigen, municipioDestino,
                horaInicialSalida, intervaloEntreBuses));
            MessageBox.Show("¡Línea añadida con éxito!");
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
