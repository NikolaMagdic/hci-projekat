using System;
using System.Collections.Generic;
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

namespace Predmetni_zadatak.Pomoć
{
    /// <summary>
    /// Interaction logic for ProzorZaPomoc.xaml
    /// </summary>
    public partial class ProzorZaPomoc : Window
    {
        public ProzorZaPomoc(string kljuc)
        {
            InitializeComponent();
            string putanja = String.Format(@"C:\Users\user\Documents\Interakcija čovek računar\Predmetni zadatak\Predmetni zadatak\Pomoć\{0}.html", kljuc);
            if (!File.Exists(putanja))
            {
                kljuc = "error";
            }
            Uri u = new Uri(String.Format(@"C:\Users\user\Documents\Interakcija čovek računar\Predmetni zadatak\Predmetni zadatak\Pomoć\{0}.html", kljuc));
            pomocPretrazivac.Navigate(u);
        }
    }
}
