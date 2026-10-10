using BusGoMobile.Models;
using BusGoMobile.Services;

namespace BusGoMobile.Pages;

[QueryProperty(nameof(TripId), "tripId")]
[QueryProperty(nameof(SeatNumber), "seatNumber")]
public partial class PassengerInfoPage : ContentPage
{
    private readonly DatabaseService _db;
    private string _gender = "Male";
    private Trip _trip;

    public int TripId { get; set; }
    public int SeatNumber { get; set; }

    public PassengerInfoPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Seferi DB'den çek ve ekranı doldur
        _trip = await _db.GetTripByIdAsync(TripId);

        if (_trip != null)
        {
            CompanyLabel.Text = _trip.CompanyName;
            RouteLabel.Text = $"{_trip.FromCity} → {_trip.ToCity}";
            DateTimeLabel.Text = $"{_trip.TravelDate}, {_trip.DepartTime}";
            PriceLabel.Text = _trip.Price.ToString();
        }

        SeatNumberLabel.Text = SeatNumber.ToString();
    }

    private void OnMaleTapped(object sender, TappedEventArgs e)
    {
        _gender = "Male";

        MaleButton.BackgroundColor = (Color)Application.Current.Resources["BgSecondaryContainer"];
        MaleIcon.TextColor = (Color)Application.Current.Resources["BgOnSecondary"];
        MaleText.TextColor = (Color)Application.Current.Resources["BgOnSecondary"];
        MaleText.FontFamily = "InterSemiBold";

        FemaleButton.BackgroundColor = (Color)Application.Current.Resources["BgSurfaceContainerLow"];
        FemaleIcon.TextColor = (Color)Application.Current.Resources["BgOnSurfaceVariant"];
        FemaleText.TextColor = (Color)Application.Current.Resources["BgOnSurfaceVariant"];
        FemaleText.FontFamily = "InterRegular";
    }

    private void OnFemaleTapped(object sender, TappedEventArgs e)
    {
        _gender = "Female";

        FemaleButton.BackgroundColor = (Color)Application.Current.Resources["BgSecondaryContainer"];
        FemaleIcon.TextColor = (Color)Application.Current.Resources["BgOnSecondary"];
        FemaleText.TextColor = (Color)Application.Current.Resources["BgOnSecondary"];
        FemaleText.FontFamily = "InterSemiBold";

        MaleButton.BackgroundColor = (Color)Application.Current.Resources["BgSurfaceContainerLow"];
        MaleIcon.TextColor = (Color)Application.Current.Resources["BgOnSurfaceVariant"];
        MaleText.TextColor = (Color)Application.Current.Resources["BgOnSurfaceVariant"];
        MaleText.FontFamily = "InterRegular";
    }

    private async void OnPayClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) ||
            string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
            string.IsNullOrWhiteSpace(PhoneEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text))
        {
            await DisplayAlert("Eksik Bilgi", "Lütfen tüm alanları doldurun.", "Tamam");
            return;
        }

        if (!TermsCheck.IsChecked)
        {
            await DisplayAlert("Onay Gerekli", "Lütfen seyahat şartlarını onaylayın.", "Tamam");
            return;
        }

        // Ödeme sayfasına git
        await Shell.Current.GoToAsync($"PaymentPage?tripId={TripId}&seatNumber={SeatNumber}");
    }

    private async void OnBackTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}