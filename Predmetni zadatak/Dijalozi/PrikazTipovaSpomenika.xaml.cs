using Predmetni_zadatak.Demo;
using Predmetni_zadatak.Model;
using Predmetni_zadatak.Pomoć;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Predmetni_zadatak.Dijalozi
{
    /// <summary>
    /// Interaction logic for PrikazTipovaSpomenika.xaml
    /// </summary>

    public partial class PrikazTipovaSpomenika : Window
    {
        public static ObservableCollection<TipSpomenika> TipoviSpomenika
        {
            get;
            set;
        }

        public ObservableCollection<TipSpomenika> TipoviFiltrirani
        {//U ovoj listi će se nalaziti svi tipovi koji nisu trenutno prikazani u tabeli
            get;
            set;
        }//Ideja je da imam dve liste: ona u kojoj se nalaze trenutno prikazane - prolaze filtraciju i ona u kojoj su neprikazane - ne ispunjavanju uslove filtracije 
        //Ovo je način da se očuva konzistentnost odnosno da se ne brišu tipovi. Tokom filtracije samo se radi prebacivanje iz jedne liste
        //u drugu. Uvek će biti prikazana lista TipoviSpomenika jer je ona ItemSource za tabelu.

        public static TipSpomenika zaPretragu;

        public PrikazTipovaSpomenika()
        {
            this.DataContext = this;
            TipoviSpomenika = MainWindow.TipoviSpomenika;
            TipoviFiltrirani = new ObservableCollection<TipSpomenika>(); //Na početku je prazna jer se svi spomenici prikazuju.
            zaPretragu = new TipSpomenika();
            InitializeComponent();
            dgrTabelaTipovaSpomenika.ItemsSource = TipoviSpomenika; //Za bindovanje
            
            Closing += this.OnWindowClosing;  //Pri zatvaranju prozora poništavaju se efekti fitracije
        }

        private void buttonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (dgrTabelaTipovaSpomenika.SelectedItem != null)
            {
                var s = new DodajTipSpomenika((TipSpomenika)dgrTabelaTipovaSpomenika.SelectedItem);
                s.ShowDialog();
            }
        }

        private void buttonObrisi_Click(object sender, RoutedEventArgs e)
        {
            if(dgrTabelaTipovaSpomenika.SelectedItem != null)
            {
                MessageBoxResult izabran = MessageBox.Show("Brisanjem tipa spomenika biće obrisani i svi spomenici koji pripadaju tom tipu. Da li želite da nastavite?","Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (izabran)
                {
                    case MessageBoxResult.Yes:
                        //Prilikom brisanja tipa spomenika svi spomenici sa tim tipom se takođe brišu; zato dodajemo upozorenje korisniku
                        TipSpomenika t = (TipSpomenika)dgrTabelaTipovaSpomenika.SelectedItem;
                        MainWindow.TipoviSpomenika.Remove(t);
                        for (int i = MainWindow.Spomenici.Count - 1; i>= 0; i--)
                        {  //foreach baca exception pa sam zato upotrebio ovaj oblik
                            Spomenik s = MainWindow.Spomenici.ElementAt(i);
                            if (s.Tip.Oznaka == t.Oznaka)
                            {
                                MainWindow.Spomenici.Remove(s);
                                MainWindow.spomeniciNaMapi.Remove(s);
                            }
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }

            }
        }

        //Filtracija
        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(radioButtonIme.IsChecked == true)
            {
                foreach (TipSpomenika ts in TipoviSpomenika) 
                {
                    if (!((ts.Ime.ToLower().Contains(textBoxFilter.Text.ToLower()))) ) //Ako ne odgovara ubaci ga u listu filtriranih
                    {
                        TipoviFiltrirani.Add(ts);  
                    }
                }
                foreach(TipSpomenika ts in TipoviFiltrirani)  //Dve for petlje jer se ne može brisati iz liste kroz koju se iterira
                {
                    TipoviSpomenika.Remove(ts);  //Sve iz liste filtriranih izbaci iz liste za prikaz
                }

                //Vraćanje iz liste filtriranih u listu za prikaz, kada se brišu slova u tekstualnom polju
                foreach(TipSpomenika ts in TipoviFiltrirani) //Prolazimo kroz listu uklonjenih
                {
                    if (ts.Ime.ToLower().Contains(textBoxFilter.Text.ToLower())) //Svaki koji je dobar vraćamo u prikazane...
                    {
                        TipoviSpomenika.Add(ts);
                    }
                }
                foreach(TipSpomenika ts in TipoviSpomenika)
                {
                    TipoviFiltrirani.Remove(ts);   //.. i izbacujemo iz skrivenih
                }

            }
            else if(radioButtonOznaka.IsChecked == true)
            {
                foreach (TipSpomenika ts in TipoviSpomenika)
                {
                    if (!ts.Oznaka.ToLower().Contains(textBoxFilter.Text.ToLower())) //Obe pretvaramo u mala slova da bi prolazile sve kombinacije malih i velikih slova
                    {
                        TipoviFiltrirani.Add(ts);
                    }
                }
                foreach (TipSpomenika ts in TipoviFiltrirani)
                {
                    TipoviSpomenika.Remove(ts);
                }

                foreach (TipSpomenika ts in TipoviFiltrirani)
                {
                    if (ts.Oznaka.ToLower().Contains(textBoxFilter.Text.ToLower()))
                    {
                        TipoviSpomenika.Add(ts);
                    }
                }
                foreach (TipSpomenika ts in TipoviSpomenika)
                {
                    TipoviFiltrirani.Remove(ts);
                }
            }

        }


        //Pretraga
        private void buttonPretraga_Click(object sender, RoutedEventArgs e)
        {
            zaPretragu.Oznaka = "";
            zaPretragu.Ime = "";

            var s = new PretragaTipova();
            s.ShowDialog();

            foreach(TipSpomenika ts in TipoviSpomenika)
            {
                if(!(ts.Oznaka.ToLower().Contains(zaPretragu.Oznaka.ToLower()) && ts.Ime.ToLower().Contains(zaPretragu.Ime.ToLower())))
                {
                    TipoviFiltrirani.Add(ts);
                }
            }
            foreach(TipSpomenika ts in TipoviFiltrirani)
            {
                TipoviSpomenika.Remove(ts);
            }

            foreach(TipSpomenika ts in TipoviFiltrirani)
            {
                if(ts.Oznaka.ToLower().Contains(zaPretragu.Oznaka.ToLower()) && ts.Ime.ToLower().Contains(zaPretragu.Ime.ToLower()))
                {
                    TipoviSpomenika.Add(ts);
                }
            }
            foreach(TipSpomenika ts in TipoviSpomenika)
            {
                TipoviFiltrirani.Remove(ts);
            }
        }

        private void buttonPonisti_Click(object sender, RoutedEventArgs e)
        {
            foreach(TipSpomenika ts in TipoviFiltrirani)
            {
                TipoviSpomenika.Add(ts);
            }
            foreach(TipSpomenika ts in TipoviSpomenika)
            {
                TipoviFiltrirani.Remove(ts);
            }
        }
        /*Pošto su MainWindow.TipoviSpomenika i TipoviSpomenika praktično samo 2 reference na isti objekat
        što smo dobili kada smo napisali TipoviSpomenika = MainWindow.TipoviSpomenika (nastala je plitka kopija) sve promene na TipoviSpomenika
        se odražavaju i na MainWindow.TipoviSpomenika, zato se pri izlasku moraju poništiti rezultati filtracije odnosno vratiti svi tipovi 
        u listu prikazanih*/
        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            foreach (TipSpomenika ts in TipoviFiltrirani)
            {
                TipoviSpomenika.Add(ts);
            }
            foreach (TipSpomenika ts in TipoviSpomenika)
            {
                TipoviFiltrirani.Remove(ts);
            }
            textBoxFilter.Text = "";  
        }

        //Demo
        private bool demoMode = false;
        public void demo_PrikazPocinje(object sender, RoutedEventArgs e)
        {
            demoMode = true;
            Point p = buttonDodaj.PointToScreen(new Point(0d, 0d));
            p.X += 20;
            p.Y += 10;

            LinearSmoothMove(p, 200); //Veći broj koraka jer je veća razdaljina pa da brzina pomeranja kursora izgleda isto

            var dts = new DodajTipSpomenika();
            dts.Show();
            Thread.Sleep(500);
            dts.beginDemo();
            //this.Close();
        }

        //U principu mogao sam da koristim i ovu funkciju iz MainWindow, ali lakše je ovako da onu ne bih pravio statičkom
        public void LinearSmoothMove(Point newPosition, int steps)
        {
            if (demoMode)
            {
                Point start = Win32.GetMousePosition();
                Point iterPoint = start;

                Point slope = new Point(newPosition.X - start.X, newPosition.Y - start.Y);

                slope.X = slope.X / steps;
                slope.Y = slope.Y / steps;

                for (int i = 0; i < steps; i++)
                {
                    if (!demoMode)
                    {
                        return;
                    }
                    iterPoint = new Point(iterPoint.X + slope.X, iterPoint.Y + slope.Y);
                    Win32.SetCursorPos((int)iterPoint.X, (int)iterPoint.Y);
                    Thread.Sleep(20);
                }

                Win32.SetCursorPos((int)newPosition.X, (int)newPosition.Y);
            }
        }

        private void ExitDemo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (demoMode == true)
            {
                e.CanExecute = true;
            }
        }

        private void ExitDemo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            demoMode = false;
        }

        private void DodajTip_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DodajTip_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var s = new DodajTipSpomenika();
            s.ShowDialog();
        }

        private void Pomoc_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Pomoc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var s = new ProzorZaPomoc("TipSpomenika");
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
