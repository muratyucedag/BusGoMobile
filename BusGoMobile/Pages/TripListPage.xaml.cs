using BusGoMobile.Services;

namespace BusGoMobile.Pages;

public partial class TripListPage : ContentPage
{

    private readonly DatabaseService _db;
    public TripListPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var seferler = await _db.GetAllTripsWithAmenitiesAsync();

        TripsView.ItemsSource = seferler;
        ResultCountLabel.Text = $"Toplam {seferler.Count} sefer bulundu";
    }

    // Bir sefere (karta veya Koltuk Seç butonuna) týklanýnca koltuk seçimine git
    private async void OnTripTapped(object sender, TappedEventArgs e)
    {
        int tripId = (int)e.Parameter;
        await Shell.Current.GoToAsync($"SeatSelectionPage?tripId={tripId}");
    }

    // Geri butonu
    private async void OnBackTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}