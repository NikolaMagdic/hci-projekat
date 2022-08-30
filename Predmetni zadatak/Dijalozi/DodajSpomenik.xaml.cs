using Microsoft.Win32;
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

namespace Predmetni_zadatak.Dijalozi
{
    /// <summary>
    /// Interaction logic for DodajSpomenik.xaml
    /// </summary>
    public partial class DodajSpomenik : Window, INotifyPropertyChanged
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
        private String opis;
        private TipSpomenika tip;
        private String klima;
        private string putanjaIkonica;
        private bool ugrozenost;
        private bool stanisteUgrozenihVrsta;
        private bool uNaseljenomRegionu;
        private String turistickiStatus;
        private double prihod;
        private DateTime datumOtkrivanja;

        bool izmena = false;
        private Spomenik zaBrisanje; //Za izmenu

        private Spomenik trenutnoIzabran; //Za proveru da li je trenutno odabran kod enable dugmeta
        private bool oznakaOK;   //Za enable i disable dugmeta Dodaj, validacija
        private bool imeOK;
        public static ObservableCollection<Etiketa> EtiketeDodate
        {
            get;
            set;
        }

        public static ObservableCollection<Etiketa> EtiketeSlobodne
        {
            get;
            set;
        }

        public static ObservableCollection<TipSpomenika> TipoviSpomenika //Za combobox tip
        {
            get;
            set;
        }

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

        public TipSpomenika Tip
        {
            get
            {
                return tip;
            }
            set
            {
                if(value != tip)
                {
                    tip = value;
                    OnPropertyChanged("Tip");
                }
            }
        }

        public String Klima
        {
            get
            {
                return klima;
            }
            set
            {
                if (value != klima)
                {
                    klima = value;
                    OnPropertyChanged("Klima");
                }
            }
        }

        public string PutanjaIkonica
        {
            get
            {
                return putanjaIkonica;
            }
            set
            {
                if (value != putanjaIkonica)
                {
                    putanjaIkonica = value;
                    OnPropertyChanged("PutanjaIkonica");
                }
            }
        }
        public bool Ugrozenost
        {
            get
            {
                return ugrozenost;
            }
            set
            {
                if (value != ugrozenost)
                {
                    ugrozenost = value;
                    OnPropertyChanged("Ugrozenost");
                }
            }
        }

        public bool Staniste
        {
            get
            {
                return stanisteUgrozenihVrsta;
            }
            set
            {
                if (value != stanisteUgrozenihVrsta)
                {
                    stanisteUgrozenihVrsta = value;
                    OnPropertyChanged("Staniste");
                }
            }
        }
        public bool Region
        {
            get
            {
                return uNaseljenomRegionu;
            }
            set
            {
                if (value != uNaseljenomRegionu)
                {
                    uNaseljenomRegionu = value;
                    OnPropertyChanged("Region");
                }
            }
        }

        public String TuristickiStatus
        {
            get
            {
                return turistickiStatus;
            }
            set
            {
                if (value != turistickiStatus)
                {
                    turistickiStatus = value;
                    OnPropertyChanged("TuristickiStatus");
                }
            }
        }
        public double Prihod
        {
            get
            {
                return prihod;
            }
            set
            {
                if (value != prihod)
                {
                    prihod = value;
                    OnPropertyChanged("Prihod");
                }
            }
        }
        public DateTime DatumOtkrivanja
        {
            get
            {
                return datumOtkrivanja;
            }
            set
            {
                if (value != datumOtkrivanja)
                {
                    datumOtkrivanja = value;
                    OnPropertyChanged("DatumOtkrivanja");
                }
            }
        }

        //Konstruktor, prazan za dodavanje novog spomenika
        public DodajSpomenik()
        {

            InitializeComponent();
            this.DataContext = this; //Za bindovanje

            comboBoxKlima.Items.Add("Polarna");
            comboBoxKlima.Items.Add("Kontinentalna");
            comboBoxKlima.Items.Add("Umereno-kontinentalna");
            comboBoxKlima.Items.Add("Pustinjska");
            comboBoxKlima.Items.Add("Subtropska");
            comboBoxKlima.Items.Add("Tropska");

            comboBoxStatus.Items.Add("Eksploatisan");
            comboBoxStatus.Items.Add("Dostupan");
            comboBoxStatus.Items.Add("Nedostupan");

            TipoviSpomenika = MainWindow.TipoviSpomenika;
            comboBoxTip.SelectedValue = MainWindow.TipoviSpomenika.ElementAt(0); //Da automatski selektovan bude prvi u listi, umesto validacije

            //Inicijalizacija lista etiketa
            EtiketeSlobodne = new ObservableCollection<Etiketa>();
            EtiketeDodate = new ObservableCollection<Etiketa>();

            foreach (Etiketa e in MainWindow.Etikete)
            {
                EtiketeSlobodne.Add(e);
            }

            izmena = false;

            buttonDodaj.IsEnabled = false;
               
        }

        public DodajSpomenik(Spomenik spomenik) //Ukoliko je u pitanju izmena ovaj konstruktor će biti pozvan
        {
            //Inicijalizacija liste etiketa ako je u pitanju izmena
            EtiketeSlobodne = new ObservableCollection<Etiketa>();
            EtiketeDodate = new ObservableCollection<Etiketa>();

            EtiketeDodate = spomenik.Etikete;
            foreach (Etiketa e in MainWindow.Etikete)
            {
                EtiketeSlobodne.Add(e);
            }
            
            foreach (Etiketa et in EtiketeDodate)
            {
                EtiketeSlobodne.Remove(et);
            }
            
            InitializeComponent();
            this.DataContext = this;

            comboBoxKlima.Items.Add("Polarna");
            comboBoxKlima.Items.Add("Kontinentalna");
            comboBoxKlima.Items.Add("Umereno-kontinentalna");
            comboBoxKlima.Items.Add("Pustinjska");
            comboBoxKlima.Items.Add("Subtropska");
            comboBoxKlima.Items.Add("Tropska");

            comboBoxStatus.Items.Add("Eksploatisan");
            comboBoxStatus.Items.Add("Dostupan");
            comboBoxStatus.Items.Add("Nedostupan");

            TipoviSpomenika = MainWindow.TipoviSpomenika;

            this.Oznaka = spomenik.Oznaka;
            this.Ime = spomenik.Ime;
            textBoxOpis.Text = spomenik.Opis;
            this.Tip = spomenik.Tip;
            comboBoxTip.SelectedValue = spomenik.Tip; //Da ispiše kao izabrani tip u comboboxu
            if(comboBoxTip.SelectedValue == null)
            {
                comboBoxTip.SelectedValue = MainWindow.TipoviSpomenika.ElementAt(0); //Ovo treba popraviti, dodato samo jer pri izmeni tipa spomenika javlja exception
            }//Ne radi ni ovako
            comboBoxKlima.Text = spomenik.Klima;
            slikaIkona.Source = new BitmapImage(new Uri(spomenik.Ikonica));
            checkBoxUgrozenost.IsChecked = spomenik.Ugrozenost;
            checkBoxStaniste.IsChecked = spomenik.Staniste;
            checkBoxRegion.IsChecked = spomenik.Region;
            comboBoxStatus.Text = spomenik.TuristickiStatus;
            this.Prihod = spomenik.Prihod;
            date.SelectedDate = spomenik.DatumOtkrivanja;
     
            izmena = true;
            zaBrisanje = spomenik;

            trenutnoIzabran = spomenik; //Za enable dugmeta

            buttonDodaj.Content = "Izmeni"; //Za izmenu prilagođavanje naziva
            this.Title = "Izmena spomenika";

        }

        private void buttonIkonica_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Izaberite sliku";
            op.Filter = "Podržani formati|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                slikaIkona.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void buttonPrebaci_Click(object sender, RoutedEventArgs e)
        {
            if(dgrTableEtiketeSlobodne.SelectedValue != null)
            {
                Etiketa zaPrebaciti = (Etiketa)dgrTableEtiketeSlobodne.SelectedValue;
                EtiketeDodate.Add(zaPrebaciti);
                EtiketeSlobodne.Remove(zaPrebaciti);
            }
        }

        private void buttonVrati_Click(object sender, RoutedEventArgs e)
        {
            if (dgrTableEtiketeDodate.SelectedValue != null)
            {
                Etiketa zaPrebaciti = (Etiketa)dgrTableEtiketeDodate.SelectedValue;
                EtiketeSlobodne.Add(zaPrebaciti);
                EtiketeDodate.Remove(zaPrebaciti);
            }
        }

        //Validacija - onemogućavanje Dodaj dugmeta ako uslovi nisu ispunjeni
        private void textBoxOznaka_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            bool jedinstvenaOznakaOK = true;
            foreach (Spomenik spom in MainWindow.Spomenici)
            {
                if (spom.Oznaka == tb.Text)
                {
                    if (this.trenutnoIzabran != null)
                    {
                        if (spom.Oznaka == trenutnoIzabran.Oznaka)
                        {
                            continue;
                        }
                    }
                    jedinstvenaOznakaOK = false;
                    break;
                }
            }
            if (tb.Text.Length < 3 || !jedinstvenaOznakaOK)
            {
                oznakaOK = false;
            }
            else
            {
                oznakaOK = true;
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

        private void textBoxIme_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Text.Length < 3)
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
                Spomenik spomenikIzmenjen = new Spomenik(textBoxOznaka.Text, textBoxIme.Text, textBoxOpis.Text, (TipSpomenika)comboBoxTip.SelectedItem, comboBoxKlima.Text, slikaIkona.Source.ToString(), (bool)checkBoxUgrozenost.IsChecked, (bool)checkBoxStaniste.IsChecked, (bool)checkBoxRegion.IsChecked, comboBoxStatus.Text, Double.Parse(textBoxPrihod.Text), date.SelectedDate.Value, EtiketeDodate);
                int idx = MainWindow.Spomenici.IndexOf(zaBrisanje); //Da ne bi dodavao na kraj nego na isto mesto gde je i bio
                MainWindow.Spomenici.Remove(zaBrisanje);
                MainWindow.Spomenici.Insert(idx, spomenikIzmenjen);
                //Dodati izmenjen u listu spomenika odgovarajućeg tipa
                TipSpomenika tip = (TipSpomenika)comboBoxTip.SelectedItem;
                //Ako je izmenjen tip spomenika izbacujemo ga iz liste spomenika prethodnog tipa
                if (!tip.Equals(zaBrisanje.Tip))
                {
                    zaBrisanje.Tip.SpomeniciTipa.Remove(zaBrisanje);
                }
                tip.SpomeniciTipa.Remove(zaBrisanje);
                tip.SpomeniciTipa.Add(spomenikIzmenjen);
                if (MainWindow.spomeniciNaMapi.Contains(zaBrisanje))
                {
                    int index = MainWindow.spomeniciNaMapi.IndexOf(zaBrisanje);
                    spomenikIzmenjen.Point = zaBrisanje.Point; //Da ostane na istoj lokaciji na mapi
                    MainWindow.spomeniciNaMapi.Remove(zaBrisanje);
                    MainWindow.spomeniciNaMapi.Insert(index, spomenikIzmenjen);
                }

                this.Close();
            }
            else
            {
                //Da podrazumevana ikonica bude ikonica tipa
                if (slikaIkona.Source == null)
                {
                    slikaIkona.Source = new BitmapImage(new Uri(((TipSpomenika)comboBoxTip.SelectedItem).PutanjaSlika)); //Tip izabran u comboboxu
                }

                Spomenik spomenik = new Spomenik(textBoxOznaka.Text, textBoxIme.Text, textBoxOpis.Text, (TipSpomenika)comboBoxTip.SelectedItem, comboBoxKlima.Text, slikaIkona.Source.ToString(), (bool)checkBoxUgrozenost.IsChecked, (bool)checkBoxStaniste.IsChecked, (bool)checkBoxRegion.IsChecked, comboBoxStatus.Text, Double.Parse(textBoxPrihod.Text), date.SelectedDate.Value, EtiketeDodate);
                MainWindow.Spomenici.Add(spomenik);
                TipSpomenika tip = (TipSpomenika)comboBoxTip.SelectedItem;
                tip.SpomeniciTipa.Add(spomenik); //Dodavanje u listu spomenika koja se nalazi u izabranom tipu spomenika
                this.Close();
            }


        }

        private void Automatski_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Automatski_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textBoxOznaka.Text = "Oznaka" + (MainWindow.Spomenici.Count + 1);
            textBoxIme.Text = "Ime" + (MainWindow.Spomenici.Count + 1);
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
