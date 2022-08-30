using Predmetni_zadatak.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Predmetni_zadatak.Serijalizacija
{
    //Mora se u References dodati using System.Runtime.Serialization
    [DataContract] //Slično kao Serializable(sa njim nisam uspeo) serijalizuje u xml
    public class Repozitorijum
    {
        [DataMember] //Označava da se ovaj član serijalizuje, ako ne bismo ubacili ne bi se sačuvala vrednost, mora se dodati za sve klase i njihove atribute
        public ObservableCollection<Spomenik> Spomenici
        {
            get;
            set;
        }

        [DataMember]
        public ObservableCollection<Etiketa> Etikete
        {
            get;
            set;
        }

        [DataMember]
        public ObservableCollection<TipSpomenika> TipoviSpomenika
        {
            get;
            set;
        }

        [DataMember]
        public ObservableCollection<Spomenik> SpomeniciNaMapi
        {
            get;
            set;
        }
    }
}
