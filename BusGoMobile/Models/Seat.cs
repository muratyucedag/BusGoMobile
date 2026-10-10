using SQLite;

namespace BusGoMobile.Models;

public enum SeatStatus
{
    Empty,
    Male,
    Female,
    Selected
}

public class Seat
{
    public int Id { get; set; }
    public int TripId { get; set; }
    public int Number { get; set; }

    // Veritabanında metin olarak tutulur: "Empty" / "Male" / "Female"
    public string Status { get; set; }

    // Koltukta oturan kişinin adı (boş koltukta NULL)
    public string PassengerName { get; set; }

    // Metni enum'a çeviren yardımcı (tablo kolonu DEĞİL)
    [Ignore]
    public SeatStatus StatusEnum
    {
        get
        {
            return Status switch
            {
                "Male" => SeatStatus.Male,
                "Female" => SeatStatus.Female,
                "Selected" => SeatStatus.Selected,
                _ => SeatStatus.Empty
            };
        }
        set
        {
            Status = value.ToString();
        }
    }

    // Boş ya da seçili koltuk seçilebilir (tablo kolonu DEĞİL)
    [Ignore]
    public bool IsSelectable => StatusEnum == SeatStatus.Empty || StatusEnum == SeatStatus.Selected;
}