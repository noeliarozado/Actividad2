using Actividad2.dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actividad2.logic
{
    public class LogicaBus
    {
        //public const string RUTA_FICHERO_CSV_LINEAS = "..\\..\\..\\data\\lineas.csv";
        //public const string RUTA_FICHERO_CSV_PARADAS = "..\\..\\..\\data\\paradas.csv";

        public const string RUTA_FICHERO_CSV_LINEAS = "data\\lineas.csv";
        public const string RUTA_FICHERO_CSV_PARADAS = "data\\paradas.csv";

        /// <summary>
        /// Lista de líneas de autobús
        /// </summary>
        private List<Linea> listaLineas;

        /// <summary>
        /// Lista de paradas de autobús
        /// </summary>
        private List<Parada> listaParadas;

        public LogicaBus()
        {
            string lineasFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                RUTA_FICHERO_CSV_LINEAS);
            string paradasFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                RUTA_FICHERO_CSV_PARADAS);

            //listaLineas = CargarCSVLineas();
            listaLineas = CargarCSVLineas(lineasFilePath);
            if (listaLineas == null)
            {
                listaLineas = new List<Linea>();
            }

            //listaParadas = CargarCSVParadas();
            listaParadas = CargarCSVParadas(paradasFilePath);
            if (listaParadas == null)
            {
                listaParadas = new List<Parada>();
            }
        }

        /// <summary>
        /// Obtiene la lista de líneas de autobús
        /// </summary>
        /// <returns>Lista de líneas</returns>
        public List<Linea> GetListaLineas()
        {
            return listaLineas;
        }

        /// <summary>
        /// Obtiene la lista de paradas de autobús
        /// </summary>
        /// <returns><Lista de paradas de autobús/returns>
        public List<Parada> GetListaParadas()
        {
            return listaParadas;
        }

        /// <summary>
        /// Añade una línea a la lista y guarda los datos en el archivo CSV
        /// </summary>
        /// <param name="linea">Línea a añadir</param>
        public void AnadirLineaALista(Linea linea)
        {
            listaLineas.Add(linea);
            GuardarCSVLineas();
        }

        /// <summary>
        /// Añade una parada a la lista y guarda los datos en el archivo CSV
        /// </summary>
        /// <param name="parada">Parada a añadir</param>
        public void AnadirParadaALista(Parada parada)
        {
            listaParadas.Add(parada);
            GuardarCSVParadas();
        }

        /// <summary>
        /// Borra una línea de la lista y guarda los cambios en el archivo CSV
        /// </summary>
        /// <param name="linea">Línea a borrar</param>
        public void BorrarLinea(Linea linea)
        {
            listaLineas.Remove(linea);
            GuardarCSVLineas();
        }

        /// <summary>
        /// Borra una parada de la lista y guarda los cambios en el archivo CSV
        /// </summary>
        /// <param name="numeroLineaSeleccionado">Parada a borrar</param>
        public void BorrarTodasParadas(int numeroLineaSeleccionado)
        {
            listaParadas.RemoveAll(stop => stop.NumeroLinea == numeroLineaSeleccionado);

            GuardarCSVParadas();
        }

        public void BorrarParada(Parada paradaABorrar)
        {
            listaParadas.Remove(paradaABorrar);
            GuardarCSVParadas();
        }

        /// <summary>
        /// Actualiza la información de una línea en el archivo CSV
        /// </summary>
        /// <param name="lineaActualizada">Línea con la información actualizada</param>
        public void ActualizarLinea(Linea lineaActualizada)
        {
            foreach (Linea linea in listaLineas)
            {
                if (linea.NumeroLinea == lineaActualizada.NumeroLinea)
                {
                    linea.NumeroLinea = lineaActualizada.NumeroLinea;
                    linea.MunicipioOrigen = lineaActualizada.MunicipioOrigen;
                    linea.MunicipioDestino = lineaActualizada.MunicipioDestino;
                    linea.HoraInicialSalida = lineaActualizada.HoraInicialSalida;
                    linea.IntervaloEntreBuses = lineaActualizada.IntervaloEntreBuses;
                    GuardarCSVLineas();
                    return;
                }
            }

            Console.WriteLine("Línea no encontrada para el número de línea: " + lineaActualizada.NumeroLinea);
        }

        /// <summary>
        /// Actualiza la información de una parada en el archivo CSV
        /// </summary>
        /// <param name="paradaActualizada">Parada con la información actualizada</param>
        public void ActualizarParada(Parada paradaActualizada, Parada paradaOriginal)
        {
            foreach (Parada parada in listaParadas)
            {
                if (parada.NumeroLinea == paradaOriginal.NumeroLinea &&
                    parada.Municipio == paradaOriginal.Municipio
                    )
                {
                    parada.NumeroLinea = paradaActualizada.NumeroLinea;
                    parada.Municipio = paradaActualizada.Municipio;
                    parada.IntervaloDesdeHoraSalida = paradaActualizada.IntervaloDesdeHoraSalida;
                    GuardarCSVParadas();
                    return;
                }
            }
            Console.WriteLine("Línea no encontrada para el número de línea: " + paradaActualizada.NumeroLinea);
        }

        /// <summary>
        /// Carga los datos desde el archivo CSV que contiene la información sobre las líneas
        /// </summary>
        /// <returns>Lista de líneas</returns>
        private List<Linea> CargarCSVLineas(string lineasFilePath)
        {
            List<Linea> lineasCargadas = new List<Linea>();
            try
            {
                using (StreamReader reader = new StreamReader(RUTA_FICHERO_CSV_LINEAS))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');
                        int NumeroLinea = int.Parse(data[0]);
                        string MunicipioOrigen = data[1];
                        string MunicipioDestino = data[2];
                        DateTime HoraInicialSalida = DateTime.Parse(data[3]);
                        TimeSpan IntervaloEntreBuses = TimeSpan.Parse(data[4]);

                        Linea linea = new Linea(NumeroLinea, MunicipioOrigen, MunicipioDestino,
                            HoraInicialSalida, IntervaloEntreBuses);
                        lineasCargadas.Add(linea);
                    }
                }
            }
            catch (IOException ioe)
            {
                Console.WriteLine("Error al leer el fichero: " + ioe.Message);
            }
            return lineasCargadas;
        }

        /// <summary>
        /// Carga los datos desde el archivo CSV que contiene la información sobre las paradas
        /// </summary>
        /// <returns>Lista de paradas</returns>
        private List<Parada> CargarCSVParadas(string paradasFilePath)
        {
            List<Parada> paradasCargadas = new List<Parada>();
            try
            {
                using (StreamReader reader = new StreamReader(RUTA_FICHERO_CSV_PARADAS))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');
                        int NumeroLinea = int.Parse(data[0]);
                        string Municipio = data[1];
                        TimeSpan IntervaloDesdeHoraSalida = TimeSpan.Parse(data[2]);

                        Parada parada = new Parada(NumeroLinea, Municipio, IntervaloDesdeHoraSalida);
                        paradasCargadas.Add(parada);
                    }
                }
            }
            catch (IOException ioe)
            {
                Console.WriteLine("Error al leer el fichero: " + ioe.Message);
            }
            return paradasCargadas;
        }

        /// <summary>
        /// Guarda los datos de la línea en el archivo CSV
        /// </summary>
        private void GuardarCSVLineas()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(RUTA_FICHERO_CSV_LINEAS))
                {
                    foreach (Linea linea in listaLineas)
                    {
                        writer.WriteLine(FormatearCSVLineas(linea));
                    }
                }
            }
            catch (IOException ioe)
            {
                Console.WriteLine("Error al escribir en el fichero: " + ioe.Message);
            }
        }

        /// <summary>
        /// Formatea los datos de una línea para incluir en un archivo CSV
        /// </summary>
        /// <param name="linea">Datos de la línea a formatear</param>
        /// <returns>Cadena con los datos de la línea según el formato de los archivos CSV</returns>
        private string FormatearCSVLineas(Linea linea)
        {
            return $"{linea.NumeroLinea},"
                + $"{linea.MunicipioOrigen},"
                + $"{linea.MunicipioDestino},"
                + $"{linea.HoraInicialSalida.ToString("HH:mm")},"
                + $"{linea.IntervaloEntreBuses}";
        }

        /// <summary>
        /// Guarda los datos de la paradas en el archivo CSV
        /// </summary>
        private void GuardarCSVParadas()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(RUTA_FICHERO_CSV_PARADAS))
                {
                    foreach (Parada parada in listaParadas)
                    {
                        writer.WriteLine(FormatStopsCSV(parada));
                    }
                }
            }
            catch (IOException ioe)
            {
                Console.WriteLine("Error al escribir en el archivo : " + ioe.Message);
            }
        }

        /// <summary>
        /// Formatea los datos de una parada para incluir en un archivo CSV
        /// </summary>
        /// <param name="parada">Datos de la parada a formatear</param>
        /// <returns>Cadena con los datos de la parada según el formato de los archivos CSV</returns>
        private string FormatStopsCSV(Parada parada)
        {
            return $"{parada.NumeroLinea},"
                + $"{parada.Municipio},"
                + $"{parada.IntervaloDesdeHoraSalida}";
        }
    }
}
