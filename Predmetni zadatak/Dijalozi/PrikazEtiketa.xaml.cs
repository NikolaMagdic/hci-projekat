using Predmetni_zadatak.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;
using Predmetni_zadatak.Pomoć;

namespace Predmetni_zadatak.Dijalozi
{
    /// <summary>
    /// Interaction logic for PrikazEtiketa.xaml
    /// </summary>
    public partial class PrikazEtiketa : Window
    {

        public static ObservableCollection<Etiketa> Etikete
        {
            get;
            set;
        }

        public ObservableCollection<Etiketa> EtiketeFiltrirane
        {
            get;
            set;
        }

        public static Etiketa zaPretragu;
        public PrikazEtiketa()
        {
            Etikete = MainWindow.Etikete;
            EtiketeFiltrirane = new ObservableCollection<Etiketa>();
            zaPretragu = new Etiketa();
            InitializeComponent();
            dgrTabelaEtiketa.ItemsSource = Etikete; //Za bindovanje

            Closing += this.OnWindowClosing;
        }

        private void buttonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (dgrTabelaEtiketa.SelectedItem != null)
            {
                var s = new DodajEtiketu((Etiketa)dgrTabelaEtiketa.SelectedItem);
                s.ShowDialog();
            }
        }

        private void buttonObrisi_Click(object sender, RoutedEventArgs e)
        {
            if(dgrTabelaEtiketa.SelectedItem != null)
            {
                MainWindow.Etikete.Remove((Etiketa)dgrTabelaEtiketa.SelectedItem);
            }
        }

        //Filtriranje sadržaja u tabeli
        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach(Etiketa et in Etikete)
            {
                if (!et.Oznaka.ToLower().Contains(textBoxFilter.Text.ToLower()))
                {
                    EtiketeFiltrirane.Add(et);
                }
            }
            foreach(Etiketa et in EtiketeFiltrirane)
            {
                Etikete.Remove(et);
            }

            //Drugi smer
            foreach (Etiketa et in EtiketeFiltrirane)
            {
                if (et.Oznaka.ToLower().Contains(textBoxFilter.Text.ToLower()))
                {
                    Etikete.Add(et);
                }
            }
            foreach (Etiketa et in Etikete)
            {
                EtiketeFiltrirane.Remove(et);
            }
        }

        //Pretraga

        private void buttonPretraga_Click(object sender, RoutedEventArgs e)
        {
            zaPretragu.Oznaka = "";

            var s = new PretragaEtiketa();
            s.ShowDialog();

            bool izabranaBoja;
            if(zaPretragu.Boja == Colors.Khaki) //Molimo boga da niko neće izabrati ovo boju, jer ko bi stavio boju koja se zove khaki?
            {
                izabranaBoja = false;
            }
            else
            {
                izabranaBoja = true;
            }

            foreach (Etiketa et in Etikete)
            {
                if (!(et.Oznaka.ToLower().Contains(zaPretragu.Oznaka.ToLower()) && (et.Boja.Equals(zaPretragu.Boja) || !izabranaBoja)))
                {
                    EtiketeFiltrirane.Add(et);
                }
            }
            foreach (Etiketa et in EtiketeFiltrirane)
            {
                Etikete.Remove(et);
            }
        }

        private void buttonPonisti_Click(object sender, RoutedEventArgs e)
        {
            foreach (Etiketa et in EtiketeFiltrirane)
            {
                Etikete.Add(et);
            }
            foreach (Etiketa et in Etikete)
            {
                EtiketeFiltrirane.Remove(et);
            }
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            textBoxFilter.Text = "";
            foreach (Etiketa et in EtiketeFiltrirane)
            {
                Etikete.Add(et);
            }
            foreach (Etiketa et in Etikete)
            {
                EtiketeFiltrirane.Remove(et);
            }
        }

        //Prečice
        private void DodajEtiketu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DodajEtiketu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var s = new DodajEtiketu();
            s.ShowDialog();
        }

        private void Pomoc_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Pomoc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var s = new ProzorZaPomoc("Etiketa");
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
