using BusGoMobile.Services;

namespace BusGoMobile.Pages;

[QueryProperty(nameof(TripId), "tripId")]
[QueryProperty(nameof(SeatNumber), "seatNumber")]
public partial class PaymentPage : ContentPage
{
    private readonly DatabaseService _db;

    public int TripId { get; set; }
    public int SeatNumber { get; set; }

    public PaymentPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var trip = await _db.GetTripByIdAsync(TripId);
        if (trip != null)
            PriceLabel.Text = trip.Price.ToString();
    }

    // Kart numarasý: 4'lü gruplar halinde biçimlendir + karta yansýt
    private void OnCardNumberChanged(object sender, TextChangedEventArgs e)
    {
        // Sadece rakamlarý al
        string digits = new string((e.NewTextValue ?? "").Where(char.IsDigit).ToArray());
        if (digits.Length > 16) digits = digits.Substring(0, 16);

        // 4'lü gruplara böl
        var gruplu = string.Join(" ", Enumerable.Range(0, (digits.Length + 3) / 4).Select(i => digits.Substring(i * 4, Math.Min(4, digits.Length - i * 4))));

        // Input'u biçimlenmiþ haliyle güncelle (sonsuz döngü olmasýn diye kontrol)
        if (CardNumberEntry.Text != gruplu)
        {
            CardNumberEntry.Text = gruplu;
            CardNumberEntry.CursorPosition = gruplu.Length;
            return; // TextChanged tekrar tetiklenecek, aþaðýsý o turda çalýþsýn
        }

        // Karta yansýt: girilen kýsmý göster, kalaný • ile doldur
        var kartGosterim = "";
        for (int i = 0; i < 16; i++)
        {
            kartGosterim += i < digits.Length ? digits[i].ToString() : "•";
            if ((i + 1) % 4 == 0 && i != 15) kartGosterim += " ";
        }
        CardNumberDisplay.Text = kartGosterim;
    }

    private void OnCardHolderChanged(object sender, TextChangedEventArgs e)
    {
        string isim = (e.NewTextValue ?? "").ToUpper();
        CardHolderDisplay.Text = string.IsNullOrWhiteSpace(isim) ? "AD SOYAD" : isim;
    }

    // Son kullanma: AA/YY biçimi + karta yansýt
    private void OnExpiryChanged(object sender, TextChangedEventArgs e)
    {
        string digits = new string((e.NewTextValue ?? "").Where(char.IsDigit).ToArray());
        if (digits.Length > 4) digits = digits.Substring(0, 4);

        string bicimli = digits;
        if (digits.Length >= 3)
            bicimli = digits.Substring(0, 2) + "/" + digits.Substring(2);

        if (ExpiryEntry.Text != bicimli)
        {
            ExpiryEntry.Text = bicimli;
            ExpiryEntry.CursorPosition = bicimli.Length;
            return;
        }

        CardExpiryDisplay.Text = string.IsNullOrWhiteSpace(bicimli) ? "AA/YY" : bicimli;
    }

    private async void OnPayClicked(object sender, EventArgs e)
    {
        string digits = new string((CardNumberEntry.Text ?? "").Where(char.IsDigit).ToArray());

        if (digits.Length < 16 ||
            string.IsNullOrWhiteSpace(CardHolderEntry.Text) ||
            string.IsNullOrWhiteSpace(ExpiryEntry.Text) ||
            string.IsNullOrWhiteSpace(CvvEntry.Text))
        {
            await DisplayAlert("Eksik Bilgi", "Lütfen tüm kart bilgilerini eksiksiz girin.", "Tamam");
            return;
        }

        await DisplayAlert("Ödeme Baþarýlý",
            $"Ödemeniz alýndý. Koltuk {SeatNumber} için biletiniz oluþturuluyor. (Bilet sayfasý sonra eklenecek.)",
            "Tamam");
    }

    private async void OnBackTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}