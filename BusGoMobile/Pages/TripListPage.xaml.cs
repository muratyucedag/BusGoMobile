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
}