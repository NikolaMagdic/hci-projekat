using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Predmetni_zadatak.Model
{
    [DataContract]
    public class Spomenik : INotifyPropertyChanged
    {
        [DataMember]
        private String oznaka;
        [DataMember]
        private String ime;
        [DataMember]
        private String opis;
        [DataMember]
        private TipSpomenika tip;
        [DataMember]
        private String klima;
        [DataMember]
        private string ikonica;
        [DataMember]
        private bool ugrozenost;
        [DataMember]
        private bool stanisteUgrozenihVrsta;
        [DataMember]
        private bool uNaseljenomRegionu;
        [DataMember]
        private String turistickiStatus;
        [DataMember]
        private double prihod;
        [DataMember]
        private DateTime datumOtkrivanja;
        [DataMember]
        private ObservableCollection<Etiketa> etikete;
        [DataMember]
        private Point point; //Za položaj na mapi za sad nisam ubacio u konstruktor

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public Spomenik() { }
        public Spomenik(String oznaka, String ime, String opis, TipSpomenika tip, String klima, string ikonica, bool ugrozenost, bool staniste, bool region, String status, double prihod, DateTime datum, ObservableCollection<Etiketa> etikete)
        {
            this.oznaka = oznaka;
            this.ime = ime;
            this.opis = opis;
            this.tip = tip;
            this.klima = klima;
            this.ikonica = ikonica;
            this.ugrozenost = ugrozenost;
            this.stanisteUgrozenihVrsta = staniste;
            this.uNaseljenomRegionu = region;
            this.turistickiStatus = status;
            this.prihod = prihod;
            this.datumOtkrivanja = datum;
            this.etikete = etikete;
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

        public string Ikonica
        {
            get
            {
                return ikonica;
            }
            set
            {
                if (value != ikonica)
                {
                    ikonica = value;
                    OnPropertyChanged("Ikonica");
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

        public ObservableCollection<Etiketa> Etikete
        {
            get
            {
                return etikete;
            }
            set
            {
                if(value != etikete)
                {
                    etikete = value;
                    OnPropertyChanged("Etikete");
                }
            }
        }

        public Point Point
        {
            get
            {
                return point;
            }
            set
            {
                if(value != point)
                {
                    point = value;
                    OnPropertyChanged("Point");
                }
            }
        }
    }
}
