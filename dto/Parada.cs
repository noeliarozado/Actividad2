using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actividad2.dto
{
    /// <summary>
    /// Clase que almacena los datos básicos de una parada de autobús
    /// </summary>
    public class Parada
    {
        /// <summary>
        /// Número de línea
        /// </summary>
        public int NumeroLinea { get; set; }

        /// <summary>
        /// Municipio
        /// </summary>
        public string Municipio { get; set; }

        /// <summary>
        /// Intervalo desde la hora de salida
        /// </summary>
        public TimeSpan IntervaloDesdeHoraSalida { get; set; }

        public Parada(int numeroLinea, string municipio, TimeSpan intervaloDesdeHoraSalida)
        {
            NumeroLinea = numeroLinea;
            Municipio = municipio;
            IntervaloDesdeHoraSalida = intervaloDesdeHoraSalida;
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
