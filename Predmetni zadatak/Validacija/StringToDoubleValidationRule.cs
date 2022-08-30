using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Predmetni_zadatak.Validacija
{
    public class StringToDoubleValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                double r;
                if (double.TryParse(s, out r))
                {
                    if (r < 0)
                        return new ValidationResult(false, "Prihod ne može biti manji od 0");
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Molimo unesite ispravnu brojnu vrednost");
            }
            catch
            {
                return new ValidationResult(false, "Nepoznata greška");
            }
            
        }
    }
}
