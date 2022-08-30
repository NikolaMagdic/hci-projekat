using Predmetni_zadatak.Model;
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

namespace Predmetni_zadatak.Dijalozi
{
    /// <summary>
    /// Interaction logic for DodajEtiketu.xaml
    /// </summary>
    public partial class DodajEtiketu : Window
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
        private Color boja;
        private String opis;

        private bool izmena = false;
        private Etiketa zaBrisanje;

        private Etiketa trenutnoIzabrana;
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

        public Color Boja
        {
            get
            {
                return boja;
            }
            set
            {
                if (value != boja)
                {
                    boja = value;
                    OnPropertyChanged("Boja");
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

        public DodajEtiketu()  //Konstruktor
        {
            InitializeComponent();
            this.DataContext = this;

            izmena = false;
            buttonDodaj.IsEnabled = false;
        }

        public DodajEtiketu(Etiketa etiketa)  //Konstruktor za izmenu
        {
            InitializeComponent();
            this.DataContext = this;

            this.Oznaka = etiketa.Oznaka;
            colorPicker.SelectedColor = etiketa.Boja;
            textBoxOpis.Text = etiketa.Opis;

            izmena = true;
            zaBrisanje = etiketa;
            this.trenutnoIzabrana = etiketa;

            buttonDodaj.Content = "Izmeni"; //Za izmenu prilagođavanje naziva
            this.Title = "Izmena etikete";
        }

        //Za disable dodaj button kod, deo validacije, događaj se okida kad god se tekst izmeni
        private void textBoxOznaka_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            bool jedinstvenaOznakaOK = true;
            foreach (Etiketa et in MainWindow.Etikete)
            {
                if (et.Oznaka == tb.Text)
                {
                    if (this.trenutnoIzabrana != null)
                    {
                        if (et.Oznaka == trenutnoIzabrana.Oznaka)
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
                buttonDodaj.IsEnabled = false;
            }
            else
            {
                buttonDodaj.IsEnabled = true;
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
                Etiketa etiketaIzmenjena = new Etiketa(textBoxOznaka.Text, colorPicker.SelectedColor.Value, textBoxOpis.Text);
                int idx = MainWindow.Etikete.IndexOf(zaBrisanje);
                MainWindow.Etikete.Remove(zaBrisanje);
                MainWindow.Etikete.Insert(idx, etiketaIzmenjena);
                this.Close();
            }
            else
            {
                //HARDKOD da se spreči exception ako nije izabrana boja
                Etiketa etiketa;
                if (colorPicker.SelectedColor == null)
                {
                    etiketa = new Etiketa(textBoxOznaka.Text, Colors.Black, textBoxOpis.Text);
                }
                else
                {
                    etiketa = new Etiketa(textBoxOznaka.Text, colorPicker.SelectedColor.Value, textBoxOpis.Text);
                }
                MainWindow.Etikete.Add(etiketa);
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
            textBoxOznaka.Text = "Oznaka" + (MainWindow.Etikete.Count + 1);
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
