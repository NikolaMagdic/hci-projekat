using Microsoft.Win32;
using Predmetni_zadatak.Demo;
using Predmetni_zadatak.Model;
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
using System.Windows.Threading;

namespace Predmetni_zadatak.Dijalozi
{
    /// <summary>
    /// Interaction logic for DodajTipSpomenika.xaml
    /// </summary>
    public partial class DodajTipSpomenika : Window
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private String oznaka;
        private String ime;
        private Image slika;
        private String opis;

        public ObservableCollection<Spomenik> spomeniciTipa;

        private bool izmena = false;
        private TipSpomenika zaBrisanje;
        private TipSpomenika trenutnoIzabran; //za validaciju, da ne bude onemogućeno polje ako je naziv isti kao i trenutnoj (praktično nema promene)

        private bool oznakaOK; //Za enable i disable dugmeta Dodaj, validacija
        private bool imeOK;

        private bool demoMode = false; //za demo

        public String Oznaka
        {
            get
            {
                return oznaka;
            }
            set
            {
                if (value != oznaka)
                {
                    oznaka = value;
                    OnPropertyChanged("Oznaka");
                }
            }
        }

        public String Ime
        {
            get
            {
                return ime;
            }
            set
            {
                if (value != ime)
                {
                    ime = value;
                    OnPropertyChanged("Ime");
                }
            }
        }

        public Image Slika
        {
            get
            {
                return slika;
            }
            set
            {
                if (value != slika)
                {
                    slika = value;
                    OnPropertyChanged("Slika");
                }
            }
        }

        public String Opis
        {
            get
            {
                return opis;
            }
            set
            {
                if (value != opis)
                {
                    opis = value;
                    OnPropertyChanged("Opis");
                }
            }
        }

        public ObservableCollection<Spomenik> SpomeniciTipa
        {
            get
            {
                return spomeniciTipa;
            }
            set
            {
                if (value != spomeniciTipa)
                {
                    spomeniciTipa = value;
                    OnPropertyChanged("SpomeniciTipa");
                }
            }
        }

        public DodajTipSpomenika()
        {
            InitializeComponent();
            this.DataContext = this;

            izmena = false;
            buttonDodaj.IsEnabled = false;

        }

        public DodajTipSpomenika(TipSpomenika tip)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Oznaka = tip.Oznaka;
            this.Ime = tip.Ime;
            slikaTip.Source = new BitmapImage(new Uri(tip.PutanjaSlika));
            textBoxOpis.Text = tip.Opis;

            izmena = true;
            zaBrisanje = tip;

            slikaObavestenje.Visibility = Visibility.Collapsed; //Ako je izmena ovo obaveštenje se ne vidi, ovo je način za to
            this.trenutnoIzabran = tip;

            buttonDodaj.Content = "Izmeni"; //Za izmenu, prilagođavanje naziva
            this.Title = "Izmena tipa";
        }

        //Dodavanje slike
        private void buttonDodajIkonu_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Izaberite sliku";
            op.Filter = "Podržani formati|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";
            if(op.ShowDialog() == true)
            {
                slikaTip.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        //Disableovanje dugmadi za dodavanje
        private void textBoxOznaka_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            bool jedinstvenaOznakaOK = true;
            foreach (TipSpomenika tip in MainWindow.TipoviSpomenika)
            {
                if(tip.Oznaka == tb.Text)
                {
                    if(this.trenutnoIzabran != null)
                    {
                        if (tip.Oznaka == trenutnoIzabran.Oznaka)
                        {
                            continue;
                        }
                    }
                    jedinstvenaOznakaOK = false;
                    break;
                }
            }
            if(tb.Text.Length < 3 || !jedinstvenaOznakaOK)
            {
                oznakaOK = false;
            }else
            {
                oznakaOK = true;
            }

            if(imeOK && oznakaOK)
            {
                buttonDodaj.IsEnabled = true;
            }else
            {
                buttonDodaj.IsEnabled = false;
            }
        }

        private void textBoxIme_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            bool jedinstvenoImeOK = true;
            foreach (TipSpomenika tip in MainWindow.TipoviSpomenika)
            {
                if (tip.Ime == tb.Text)
                {
                    if (this.trenutnoIzabran != null)
                    {
                        if (tip.Ime == trenutnoIzabran.Ime)
                        {
                            continue;
                        }
                    }
                    jedinstvenoImeOK = false;
                    break;
                }
            }
            if (tb.Text.Length < 3 || !jedinstvenoImeOK)
            {
                imeOK = false;
            }
            else
            {
                imeOK = true;
            }

            if (imeOK && oznakaOK)
            {
                buttonDodaj.IsEnabled = true;
            }
            else
            {
                buttonDodaj.IsEnabled = false;
            }
        }

        //Prečice

        private void Potvrdi_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Potvrdi_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (izmena)
            {
                TipSpomenika tipSpomenikaIzmenjen = new TipSpomenika(textBoxOznaka.Text, textBoxIme.Text, slikaTip.Source.ToString(), textBoxOpis.Text);
                // Prevezivanje liste spomenika u izmenjenu listu spomenika, pošto se zajedno sa  listom Tipova birše i njena lista spomenika
                tipSpomenikaIzmenjen.SpomeniciTipa = zaBrisanje.SpomeniciTipa;
                int idx = MainWindow.TipoviSpomenika.IndexOf(zaBrisanje);
                MainWindow.TipoviSpomenika.Remove(zaBrisanje);
                MainWindow.TipoviSpomenika.Insert(idx, tipSpomenikaIzmenjen);
                //Rešen bag da kada se izmeni tip spomenika svim spomenicima tog tipa se doda izmenjeni tip
                foreach (Spomenik s in MainWindow.Spomenici)
                {
                    if (s.Tip.Equals(zaBrisanje))
                    {
                        s.Tip = tipSpomenikaIzmenjen;
                    }
                }
                this.Close();
            }
            else //Prvi put dodajemo, nije izmena
            {   //HARDKOD ako se ne izabere slika biće postavljena difoltna
                if (slikaTip.Source == null)
                {
                    slikaTip.Source = new BitmapImage(new Uri("/slike/forest48.png", UriKind.Relative)); //Relative jer ga uzimamo iz projekta
                }
                TipSpomenika tipSpomenika = new TipSpomenika(textBoxOznaka.Text, textBoxIme.Text, slikaTip.Source.ToString(), textBoxOpis.Text);
                tipSpomenika.SpomeniciTipa = new ObservableCollection<Spomenik>(); //Prazna lista spomenika se dodaje samo prvi put kada se formira novi tip
                MainWindow.TipoviSpomenika.Add(tipSpomenika);
                this.Close();
            }


        }

        //Automatizacija
        private void Automatski_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Automatski_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textBoxOznaka.Text = "Oznaka" + (MainWindow.TipoviSpomenika.Count + 1);
            textBoxIme.Text = "Ime" + (MainWindow.TipoviSpomenika.Count + 1);
        }


        private void Izlaz_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Izlaz_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }


        #region Demo mode

        public Point getOznakaPoint()  //Vraća tačku koja se nalazi u textBox-u Oznaka
        {
            if (!demoMode)         
                return new Point(0, 0);
            return textBoxOznaka.PointToScreen(new Point(0d, 0d));
        }

        public Point getImePoint()
        {
            if (!demoMode)
                return new Point(0, 0);
            Point p2 = textBoxIme.PointToScreen(new Point(0d, 0d));
            return p2;
        }

        public Point getOpisPoint()
        {
            if (!demoMode)
                return new Point(0, 0);
            return textBoxOpis.PointFromScreen(new Point(0d, 0d));
        }

        public Point getOdustaniPoint()
        {
            if (!demoMode)
                return new Point(0, 0);
            return buttonOdustani.PointToScreen(new Point(0d, 0d));
        }

        public void beginDemo()
        {
            demoMode = true;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            { //Dispatcher - služi nam da napravi novi thread(u kom se obavlja demo) koji će da radi nezavisno od glavnog kojim upravlja korisnik 
                Point p3 = getOznakaPoint();
                p3.Y += 5;
                p3.X += 50;
                LinearSmoothMove(p3, 100);
                System.Threading.Thread.Sleep(500);

            });
            //Popunjavanje tekstualnog polja oznaka, igleda kao da se popunjava slovo po slovo premda mi samo u različitim vremenskim trenucima
            if (!demoMode) // menjamo prikaz sadržaja tekstualnog polja
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxOznaka.Text = "O";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxOznaka.Text = "Oz";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxOznaka.Text = "Ozn";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxOznaka.Text = "Ozna";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxOznaka.Text = "Oznak";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxOznaka.Text = "Oznaka";

            });
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxOznaka.Text = "Oznaka1";

            });//Ponavljamo još jednom poslednji unos jer nekad hoće da proguta poslednje slovo pa ga doda kasnije
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate //Pretpostavljam da je problem u preklapanju niti
            {
                System.Threading.Thread.Sleep(100);
                textBoxOznaka.Text = "Oznaka1";

            });

            if (!demoMode)
                return;  //Stalno proveravamo da li je demoMode uključen jer ga korisnik može u svakom trenutku islključiti

            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(500);
                Point p2 = getImePoint();
                p2.Y += 5;
                p2.X += 50;
                LinearSmoothMove(p2, 50);
                System.Threading.Thread.Sleep(500);
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxIme.Text = "I";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxIme.Text = "Im";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxIme.Text = "Ime";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxIme.Text = "Ime1";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxIme.Text = "Ime1";
            });
            if (!demoMode)
                return;

            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                Point p3 = getOpisPoint();
                p3.Y = 0 - p3.Y; //Ovde iz nekog razloga pomera na suprotnu stranu pa pretvaramo u negativno
                p3.X = 0 - p3.X;

                p3.X += 50;
                p3.Y += 10;
                LinearSmoothMove(p3, 100);
                System.Threading.Thread.Sleep(500);
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxOpis.Text = "O";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxOpis.Text = "Op";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxOpis.Text = "Opi";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxOpis.Text = "Opis";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxOpis.Text = "Opis1";
            });
            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(100);
                textBoxOpis.Text = "Opis1";
            });
            if (!demoMode)
                return;
            
            // na odustani
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(500);
                Point p2 = getOdustaniPoint();
                p2.Y += 5;
                p2.X += 20;
                LinearSmoothMove(p2, 100);
                System.Threading.Thread.Sleep(500);
            });

            if (!demoMode)
                return;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                System.Threading.Thread.Sleep(2000);
                this.Close();
                Application.Current.MainWindow.Show();
            });
        }

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
                    System.Threading.Thread.Sleep(20);
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
            this.Close();
            Application.Current.MainWindow.Show();
        }
        #endregion


    }
}
