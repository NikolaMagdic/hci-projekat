using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Predmetni_zadatak.Validacija
{
    public class StringValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                if(s.Length > 2)
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Oznaka mora imati najmanje tri slova");
            }
            catch
            {
                return new ValidationResult(false, "Nepoznata greška");
            }
        }
    }
}
