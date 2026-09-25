using SQLite;

namespace BusGoMobile.Models;

public class Trip
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string CompanyColor { get; set; }
    public string SeatLayout { get; set; }
    public string FromCity { get; set; }
    public string FromTerminal { get; set; }
    public string ToCity { get; set; }
    public string ToTerminal { get; set; }
    public string DepartTime { get; set; }
    public string ArriveTime { get; set; }
    public string Duration { get; set; }
    public int Price { get; set; }
    public string SeatInfo { get; set; }
    public string TravelDate { get; set; }
    public string RouteNote { get; set; }

    // "#b91c1c" -> Color (firma logosu rengi için)
    public Color CompanyColorAsColor
    {
        get
        {
            if (string.IsNullOrWhiteSpace(CompanyColor))
                return Colors.Gray;
            return Color.FromArgb(CompanyColor);
        }
    }

    // Fiyatı "520 TL" gibi göstermek için
    public string PriceText => $"{Price} TL";

    // Bu seferin olanakları (veritabanından doldurulacak, tablo kolonu DEĞİL)
    [Ignore]
    public List<Amenity> AmenityList { get; set; } = new();

    // En ucuz sefer mi? (kodda set edilir, tablo kolonu DEĞİL)
    [Ignore]
    public bool IsCheapest { get; set; }
}