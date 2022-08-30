using Predmetni_zadatak.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Predmetni_zadatak.Validacija
{
    class JedinstvenoImeTipValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                foreach (TipSpomenika tip in MainWindow.TipoviSpomenika)
                {
                    if (tip.Ime.Equals(s))
                    {
                        return new ValidationResult(false, "Naziv mora biti jedinstven!");
                    }
                }
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "Nepoznata greška");
            }
        }

    }
}
