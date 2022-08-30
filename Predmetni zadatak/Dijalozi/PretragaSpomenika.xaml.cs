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
    /// Interaction logic for PretragaSpomenika.xaml
    /// </summary>
    public partial class PretragaSpomenika : Window
    {
        private Spomenik spomenik;
        public PretragaSpomenika()
        {
            InitializeComponent();
            comboBoxKlima.Items.Add("Polarna");
            comboBoxKlima.Items.Add("Kontinentalna");
            comboBoxKlima.Items.Add("Umereno-kontinentalna");
            comboBoxKlima.Items.Add("Pustinjska");
            comboBoxKlima.Items.Add("Subtropska");
            comboBoxKlima.Items.Add("Tropska");

            comboBoxStatus.Items.Add("Eksploatisan");
            comboBoxStatus.Items.Add("Dostupan");
            comboBoxStatus.Items.Add("Nedostupan");

            foreach(TipSpomenika tip in MainWindow.TipoviSpomenika)
            {
                comboBoxTip.Items.Add(tip.Ime);
            }
        }

        private void buttonPretrazi_Click(object sender, RoutedEventArgs e)
        {
            spomenik = new Spomenik();
            spomenik.Oznaka = textBoxOznaka.Text;
            spomenik.Ime = textBoxIme.Text;
            foreach(TipSpomenika tip in MainWindow.TipoviSpomenika)
            {
                if (tip.Ime.Equals(comboBoxTip.SelectedItem))
                {
                    spomenik.Tip = tip;
                    break;
                }
            }
            spomenik.Klima = comboBoxKlima.Text;
            spomenik.Ugrozenost = (bool)checkBoxUgrozenost.IsChecked;
            spomenik.Staniste = (bool)checkBoxStaniste.IsChecked;
            spomenik.Region = (bool)checkBoxRegion.IsChecked;
            spomenik.TuristickiStatus = comboBoxStatus.Text;
            if((textBoxPrihod.Text != ""))
            {
                try
                {
                    spomenik.Prihod = Double.Parse(textBoxPrihod.Text);
                }
                catch(FormatException f)
                {
                    
                }

            }
            if(datumPolje.SelectedDate != null)
            {
                spomenik.DatumOtkrivanja = datumPolje.SelectedDate.Value;
            }
            else
            {
                spomenik.DatumOtkrivanja = DateTime.Now.Date;
            }

            PrikazSpomenika.zaPretragu = spomenik;

            Close();
        }
    }
}
