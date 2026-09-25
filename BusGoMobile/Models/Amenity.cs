using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusGoMobile.Models
{
    public class Amenity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconCode { get; set; }

        // "e541" -> gerçek ikon karakteri
        public string IconGlyph
        {
            get
            {
                if (string.IsNullOrWhiteSpace(IconCode))
                    return "";
                int code = Convert.ToInt32(IconCode, 16);
                return char.ConvertFromUtf32(code);
            }
        }
    }
}
