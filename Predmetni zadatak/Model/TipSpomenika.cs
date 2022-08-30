using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Predmetni_zadatak.Model
{
    [DataContract]
    public class TipSpomenika : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        [DataMember]
        private String oznaka;
        [DataMember]
        private String ime;
        [DataMember]
        private string putanjaSlika;
        [DataMember]
        private String opis;

        //Lista spomenika koji pripadaju navedenom tipu. Potrebno za prikaz u stablu.
        [DataMember]
        private ObservableCollection<Spomenik> spomeniciTipa;

        public TipSpomenika() { } //Prazan konstruktor je neophodan zbog serijalizacije

        public TipSpomenika(String oznaka, String ime, string putanjaSlika, String opis)
        {
            this.oznaka = oznaka;
            this.ime = ime;
            this.putanjaSlika = putanjaSlika;
            this.opis = opis;
        }

        public String Oznaka
        {
            get
            {
                return oznaka;
            }
            set
            {
                if(value != oznaka)
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
                if(value != ime)
                {
                    ime = value;
                    OnPropertyChanged("Ime");
                }
            }
        }

        public string PutanjaSlika
        {
            get
            {
                return putanjaSlika;
            }
            set
            {
                if (value != putanjaSlika)
                {
                    putanjaSlika = value;
                    OnPropertyChanged("PutanjaSlika");
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
    }
}
