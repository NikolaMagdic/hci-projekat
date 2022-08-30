using Predmetni_zadatak.Model;
using Predmetni_zadatak.Pomoć;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Predmetni_zadatak.Dijalozi
{
    /// <summary>
    /// Interaction logic for PrikazSpomenika.xaml
    /// </summary>
    public partial class PrikazSpomenika : Window
    {

        public static ObservableCollection<Spomenik> Spomenici
        {
            get;
            set;
        }

        public ObservableCollection<Spomenik> SpomeniciFiltrirani
        {
            get;
            set;
        }

        public static Spomenik zaPretragu;
        public PrikazSpomenika()
        {
            Spomenici = MainWindow.Spomenici;
            SpomeniciFiltrirani = new ObservableCollection<Spomenik>();
            zaPretragu = new Spomenik();
            InitializeComponent();
            dgrTabelaSpomenika.ItemsSource = Spomenici;  //Ova linija je ključna da bi se bindovala tabela

            Closing += this.OnWindowClosing;
        }

        private void buttonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (dgrTabelaSpomenika.SelectedItem != null)
            {
                var s = new DodajSpomenik((Spomenik)dgrTabelaSpomenika.SelectedItem);
                s.ShowDialog();
            }
        }

        private void buttonObrisi_Click(object sender, RoutedEventArgs e)
        {
            if(dgrTabelaSpomenika.SelectedItem != null)
            {
                ((Spomenik)dgrTabelaSpomenika.SelectedItem).Tip.SpomeniciTipa.Remove((Spomenik)dgrTabelaSpomenika.SelectedItem);
                MainWindow.spomeniciNaMapi.Remove((Spomenik)dgrTabelaSpomenika.SelectedItem);
                MainWindow.Spomenici.Remove((Spomenik)dgrTabelaSpomenika.SelectedItem);
            }//Briše u tabeli, na mapi i u stablu
        }


        //Filtriranje
        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (radioButtonOznaka.IsChecked == true)
            {
                foreach (Spomenik s in Spomenici)  
                {
                    if (!s.Oznaka.ToLower().Contains(textBoxFilter.Text.ToLower())) 
                    {
                        SpomeniciFiltrirani.Add(s);
                    }
                }
                foreach (Spomenik s in SpomeniciFiltrirani)  
                {
                    Spomenici.Remove(s);  
                }

                
                foreach (Spomenik s in SpomeniciFiltrirani) 
                {
                    if (s.Oznaka.ToLower().Contains(textBoxFilter.Text.ToLower())) 
                    {
                        Spomenici.Add(s);
                    }
                }
                foreach (Spomenik s in Spomenici)
                {
                    SpomeniciFiltrirani.Remove(s);   
                }

            }
            //po imenu
            else if (radioButtonIme.IsChecked == true)
            {
                foreach (Spomenik s in Spomenici)
                {
                    if (!s.Ime.ToLower().Contains(textBoxFilter.Text.ToLower()))
                    {
                        SpomeniciFiltrirani.Add(s);
                    }
                }
                foreach (Spomenik s in SpomeniciFiltrirani)
                {
                    Spomenici.Remove(s);
                }

                foreach (Spomenik s in SpomeniciFiltrirani)
                {
                    if (s.Ime.ToLower().Contains(textBoxFilter.Text.ToLower()))
                    {
                        Spomenici.Add(s);
                    }
                }
                foreach (Spomenik s in Spomenici)
                {
                    SpomeniciFiltrirani.Remove(s);
                }
            }
            //po tipu
            else if (radioButtonTip.IsChecked == true)
            {
                foreach (Spomenik s in Spomenici)
                {
                    if (!s.Tip.Ime.ToLower().Contains(textBoxFilter.Text.ToLower()))
                    {
                        SpomeniciFiltrirani.Add(s);
                    }
                }
                foreach (Spomenik s in SpomeniciFiltrirani)
                {
                    Spomenici.Remove(s);
                }

                foreach (Spomenik s in SpomeniciFiltrirani)
                {
                    if (s.Tip.Ime.ToLower().Contains(textBoxFilter.Text.ToLower()))
                    {
                        Spomenici.Add(s);
                    }
                }
                foreach (Spomenik s in Spomenici)
                {
                    SpomeniciFiltrirani.Remove(s);
                }
            }
        }

        //Pretraga
        private void buttonPretraga_Click(object sender, RoutedEventArgs e)
        {
            zaPretragu.Oznaka = "";
            zaPretragu.Ime = "";
            zaPretragu.Klima = "";
            zaPretragu.TuristickiStatus = "";
            zaPretragu.Prihod = 0;
            zaPretragu.DatumOtkrivanja = DateTime.Now.Date;

            var s = new PretragaSpomenika();
            s.ShowDialog();

            bool upisanPrihod;
            if (zaPretragu.Prihod == 0)
            {
                upisanPrihod = false;
            }
            else
            {
                upisanPrihod = true;
            }

            bool imaDatum;
            if(zaPretragu.DatumOtkrivanja != DateTime.Now.Date)
            {
                imaDatum = true;
            }
            else
            {
                imaDatum = false;
            }

            foreach (Spomenik sp in Spomenici)
            {
                if (!(sp.Oznaka.ToLower().Contains(zaPretragu.Oznaka.ToLower()) && sp.Ime.ToLower().Contains(zaPretragu.Ime.ToLower())
                    && sp.Klima.Contains(zaPretragu.Klima) && sp.TuristickiStatus.Contains(zaPretragu.TuristickiStatus)
                    && ((!upisanPrihod) || sp.Prihod == zaPretragu.Prihod) && (sp.Ugrozenost == zaPretragu.Ugrozenost || (!zaPretragu.Ugrozenost)) && (sp.Staniste == zaPretragu.Staniste ||(!zaPretragu.Staniste))
                    && ((sp.Region == zaPretragu.Region) || (!zaPretragu.Region)) && ((!imaDatum) || (zaPretragu.DatumOtkrivanja.Equals(sp.DatumOtkrivanja)))))
                {
                    SpomeniciFiltrirani.Add(sp);
                }
            }
            foreach (Spomenik sp in SpomeniciFiltrirani)
            {
                Spomenici.Remove(sp);
            }

            foreach (Spomenik sp in SpomeniciFiltrirani)
            {
                if (sp.Oznaka.ToLower().Contains(zaPretragu.Oznaka.ToLower()) && sp.Ime.ToLower().Contains(zaPretragu.Ime.ToLower())
                    && sp.Klima.Contains(zaPretragu.Klima) && sp.TuristickiStatus.Contains(zaPretragu.TuristickiStatus) && (sp.Prihod == zaPretragu.Prihod)
                    && (sp.Ugrozenost == zaPretragu.Ugrozenost) && (sp.Staniste == zaPretragu.Staniste) && (sp.Region == zaPretragu.Region))
                {
                    Spomenici.Add(sp);
                }
            }
            foreach (Spomenik sp in Spomenici)
            {
                SpomeniciFiltrirani.Remove(sp);
            }
        }

        private void buttonPonisti_Click(object sender, RoutedEventArgs e)
        {
            foreach (Spomenik s in SpomeniciFiltrirani)
            {
                Spomenici.Add(s);
            }
            foreach (Spomenik s in Spomenici)
            {
                SpomeniciFiltrirani.Remove(s);
            }
        }


        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            textBoxFilter.Text = "";
            foreach (Spomenik s in SpomeniciFiltrirani)
            {
                Spomenici.Add(s);
            }
            foreach (Spomenik s in Spomenici)
            {
                SpomeniciFiltrirani.Remove(s);
            }
        }

        //Prečice

        private void DodajSpomenik_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DodajSpomenik_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Upozorenje ako nema prethodno dodatih tipova, da se moraju dodati, inače bi bio exception jer je u combobox-u za dodavanje stavljen prvi da je odabran po difoltu
            if (MainWindow.TipoviSpomenika.Count < 1)
            {
                MessageBox.Show("Da biste mogli da dodajete spomenike potrebno je da prvo dodate bar jedan tip spomenika.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var s = new DodajSpomenik();
            s.ShowDialog();
        }

        private void Pomoc_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Pomoc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var s = new ProzorZaPomoc("Spomenik");
            s.ShowDialog();
        }


        private void Izlaz_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Izlaz_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

    }
}
