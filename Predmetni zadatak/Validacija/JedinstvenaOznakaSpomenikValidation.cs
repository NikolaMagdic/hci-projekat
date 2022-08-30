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
    class JedinstvenaOznakaSpomenikValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                foreach (Spomenik spomenik in MainWindow.Spomenici)
                {
                    if (spomenik.Oznaka.Equals(s))
                    {
                        return new ValidationResult(false, "Oznaka mora biti jedinstvena!");
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