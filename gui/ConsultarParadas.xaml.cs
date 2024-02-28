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
    public partial class ConsultarParadas : Window, INotifyPropertyChanged
    {
        private LogicaBus logicaBus = new LogicaBus();

        private List<Parada> paradas;

        public List<Parada> Paradas
        {
            get { return paradas; }
            set { paradas = value; OnPropertyChanged(nameof(paradas)); }
        }
        public ConsultarParadas()
        {
            InitializeComponent();

            DataContext = this;

            CargarParadasOrdenadas();
        }

        private void CargarParadasOrdenadas()
        {
            List<Parada> paradas = logicaBus.GetListaParadas();

            Paradas = paradas.OrderBy(stop => stop.NumeroLinea)
                            .ThenBy(stop => stop.IntervaloDesdeHoraSalida)
                            .ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
