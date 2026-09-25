using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusGoMobile.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Terminal { get; set; }

        // Picker'da "İstanbul (Esenler Otogarı)" gibi görünmesi için
        public string DisplayName =>
            string.IsNullOrWhiteSpace(Terminal) ? Name : $"{Name} ({Terminal})";

    }
}
