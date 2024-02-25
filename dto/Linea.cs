using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actividad2.dto
{
    /// <summary>
    /// Clase que almacena los datos básicos de una línea de autobús
    /// </summary>
    public class Linea : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Número de línea
        /// </summary>
        private int numeroLinea;
        public int NumeroLinea
        {
            get { return numeroLinea; }
            set { numeroLinea = value; OnPropertyChanged(nameof(NumeroLinea)); }
        }

        /// <summary>
        /// Municipio origen
        /// </summary>
        private string municipioOrigen;
        public string MunicipioOrigen
        {
            get { return municipioOrigen; }
            set { municipioOrigen = value; OnPropertyChanged(nameof(MunicipioOrigen)); }
        }

        /// <summary>
        /// Municipio destino
        /// </summary>
        private string municipioDestino;
        public string MunicipioDestino
        {
            get { return municipioDestino; }
            set { municipioDestino = value; OnPropertyChanged(nameof(MunicipioDestino)); }
        }

        /// <summary>
        /// Hora inicial de salida
        /// </summary>
        private DateTime horaInicialSalida;
        public DateTime HoraInicialSalida
        {
            get { return horaInicialSalida; }
            set { horaInicialSalida = value; OnPropertyChanged(nameof(HoraInicialSalida)); }
        }

        /// <summary>
        /// Intervalo entre buses
        /// </summary>
        private TimeSpan intervaloEntreBuses;
        public TimeSpan IntervaloEntreBuses
        {
            get { return intervaloEntreBuses; }
            set { intervaloEntreBuses = value; OnPropertyChanged(nameof(IntervaloEntreBuses)); }
        }

        public Linea(int numeroLinea, string municipioOrigen, string municipioDestino,
            DateTime horaInicialSalida, TimeSpan intervaloEntreBuses)
        {
            NumeroLinea = numeroLinea;
            MunicipioOrigen = municipioOrigen;
            MunicipioDestino = municipioDestino;
            HoraInicialSalida = horaInicialSalida;
            IntervaloEntreBuses = intervaloEntreBuses;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int getNumeroLinea()
        {
            return NumeroLinea;
        }

        public override string ToString()
        {
            return NumeroLinea.ToString();
        }
    }
}
