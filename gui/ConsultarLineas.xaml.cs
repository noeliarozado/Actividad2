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
    public partial class ConsultarLineas : Window, INotifyPropertyChanged
    {
        private LogicaBus logicaBus = new LogicaBus();

        private List<Linea> lineas;
        public List<Linea> Lineas
        {
            get { return lineas; }
            set { lineas = value; OnPropertyChanged(nameof(Lineas)); }
        }

        public ConsultarLineas()
        {
            InitializeComponent();

            DataContext = this;

            CargarLineas();
        }

        private void CargarLineas()
        {
            Lineas = logicaBus.GetListaLineas().OrderBy(line => line.NumeroLinea).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
