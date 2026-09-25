using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusGoMobile.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconCode { get; set; }

        // Tabloda "e530" tutuyoruz; XAML'de göstermek için gerçek karaktere çeviriyoruz.
        public string IconGlyph
        {
            get
            {
                if (string.IsNullOrWhiteSpace(IconCode))
                    return "";

                // "e530" gibi hex kodu gerçek unicode karaktere çevir
                int code = Convert.ToInt32(IconCode, 16);
                return char.ConvertFromUtf32(code);
            }
        }

    }
}
