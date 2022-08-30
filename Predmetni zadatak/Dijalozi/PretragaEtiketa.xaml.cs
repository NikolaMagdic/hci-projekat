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
    /// Interaction logic for PretragaEtiketa.xaml
    /// </summary>
    public partial class PretragaEtiketa : Window
    {
        private Etiketa etiketa;
        public PretragaEtiketa()
        {
            InitializeComponent();
        }

        private void buttonPretrazi_Click(object sender, RoutedEventArgs e)
        {
            etiketa = new Etiketa();
            etiketa.Oznaka = textBoxOznaka.Text;
            if (colorPicker.SelectedColor != null)
            {
                etiketa.Boja = colorPicker.SelectedColor.Value;
            }
            else
            {
                etiketa.Boja = Colors.Khaki; //neka random boja za koju se molimo da je niko neće izabrati
            }
         
            PrikazEtiketa.zaPretragu = etiketa;

            Close();
        }
    }
}
