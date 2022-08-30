using Predmetni_zadatak.Model;
using System;
using System.Collections.Generic;
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

namespace Predmetni_zadatak.Dijalozi
{
    /// <summary>
    /// Interaction logic for PretragaTipova.xaml
    /// </summary>
    public partial class PretragaTipova : Window
    {

        TipSpomenika tip;
        
        public PretragaTipova()
        {
            InitializeComponent();

        }

        private void buttonPretrazi_Click(object sender, RoutedEventArgs e)
        {
            tip = new TipSpomenika();
            tip.Oznaka = textBoxOznaka.Text;
            tip.Ime = textBoxIme.Text;
            PrikazTipovaSpomenika.zaPretragu = tip;

            Close();       
        }
    }
}
