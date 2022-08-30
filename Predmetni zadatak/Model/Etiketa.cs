using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Predmetni_zadatak.Model
{
    [DataContract]
    public class Etiketa : INotifyPropertyChanged
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
        private Color boja;
        [DataMember]
        private String opis;
        
        public Etiketa() { }
        public Etiketa(String oznaka, Color boja, String opis)
        {
            this.oznaka = oznaka;
            this.boja = boja;
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

        public Color Boja
        {
            get
            {
                return boja;
            }
            set
            {
                if(value != boja)
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
                if(value != opis)
                {
                    opis = value;
                    OnPropertyChanged("Opis");
                }
            }
        }
    }
}
