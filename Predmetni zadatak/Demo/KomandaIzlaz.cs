using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Predmetni_zadatak.Demo
{
    public class KomandaIzlaz
    {
        public static readonly RoutedUICommand ExitDemo = new RoutedUICommand(
            "Exit demo", "ExitDemo",
            typeof(KomandaIzlaz),
            new InputGestureCollection()
            {
                    new KeyGesture(Key.Space)
            }
        );
    }
}
