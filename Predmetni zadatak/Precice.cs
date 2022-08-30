using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Predmetni_zadatak
{
    public class Precice
    {
        public static readonly RoutedUICommand DodajSpomenik = new RoutedUICommand(
        "Dodaj spomenik", "DodajSpomenik",
        typeof(Precice),
        new InputGestureCollection()
        {
            new KeyGesture(Key.S, ModifierKeys.Control)
        }
            
        );

        public static readonly RoutedUICommand DodajTip = new RoutedUICommand(
        "Dodaj tip", "DodajTip",
        typeof(Precice),
        new InputGestureCollection()
        {
            new KeyGesture(Key.T, ModifierKeys.Control)
        }
        );

        public static readonly RoutedUICommand DodajEtiketu = new RoutedUICommand(
        "Dodaj Etiketu", "DodajEtiketu",
        typeof(Precice),
        new InputGestureCollection()
        {
             new KeyGesture(Key.E, ModifierKeys.Control)
        }
        );

        public static readonly RoutedUICommand ObrisiSpomenik = new RoutedUICommand(
        "Obrisi spomenik", "ObrisiSpomenik",
        typeof(Precice),
        new InputGestureCollection()
        {
               new KeyGesture(Key.O, ModifierKeys.Control)
        }
        );

        public static readonly RoutedUICommand ObrisiTip = new RoutedUICommand(
        "Obrisi tip", "ObrisiTip",
        typeof(Precice),
        new InputGestureCollection()
        {
               new KeyGesture(Key.B, ModifierKeys.Control)
        }
        );

        public static readonly RoutedUICommand ObrisiEtiketu = new RoutedUICommand(
        "Obrisi etiketu", "ObrisiEtiketu",
        typeof(Precice),
        new InputGestureCollection()
        {
               new KeyGesture(Key.R, ModifierKeys.Control)
        }
        );

        public static readonly RoutedUICommand PrikaziSpomenik = new RoutedUICommand(
        "Prikazi spomenik", "PrikaziSpomenik",
        typeof(Precice),
        new InputGestureCollection()
        {
               new KeyGesture(Key.P, ModifierKeys.Control)
        }
        );

        public static readonly RoutedUICommand PrikaziTip = new RoutedUICommand(
        "PrikaziTip", "PrikaziTip",
        typeof(Precice),
        new InputGestureCollection()
        {
               new KeyGesture(Key.K, ModifierKeys.Control)
        }
        );

        public static readonly RoutedUICommand PrikaziEtiketu = new RoutedUICommand(
        "Prikazi etiketu", "PrikaziEtiketu",
        typeof(Precice),
        new InputGestureCollection()
        {
               new KeyGesture(Key.Z, ModifierKeys.Control)
        }
        );

        public static readonly RoutedUICommand Pomoc = new RoutedUICommand(
        "Pomoc", "Pomoc",
        typeof(Precice),
        new InputGestureCollection()
        {
               new KeyGesture(Key.H, ModifierKeys.Control)
        }
        );

        public static readonly RoutedUICommand Automatski = new RoutedUICommand(
        "Automatski", "Automatski",
        typeof(Precice),
        new InputGestureCollection()
        {
               new KeyGesture(Key.A, ModifierKeys.Control)
        }
        );

        public static readonly RoutedUICommand Izlaz = new RoutedUICommand(
        "Izlaz", "Izlaz",
        typeof(Precice),
        new InputGestureCollection()
        {
               new KeyGesture(Key.Escape)
        }
        );

        public static readonly RoutedUICommand Potvrdi = new RoutedUICommand(
        "Potvrdi", "Potvrdi",
        typeof(Precice),
        new InputGestureCollection()
        {
               new KeyGesture(Key.Enter)
        }
        );

        public static readonly RoutedUICommand ObrisiNaMapi = new RoutedUICommand(
        "Obrisi na mapi", "ObrisiNaMapi",
        typeof(Precice),
        new InputGestureCollection()
        {
               new KeyGesture(Key.Delete)
        }
        );
    }
}
