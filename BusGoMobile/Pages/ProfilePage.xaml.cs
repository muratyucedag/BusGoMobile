using BusGoMobile.Services;

namespace BusGoMobile.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly DatabaseService _db;

    public ProfilePage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        int userId = Preferences.Get("UserId", 0);
        if (userId != 0)
        {
            var user = await _db.GetUserByIdAsync(userId);
            if (user != null)
            {
                UserNameLabel.Text = $"{user.FirstName} {user.LastName}";
                UserEmailLabel.Text = user.Email;
                return;
            }
        }

        // Kullanýcý bulunamazsa Preferences'tan yedek
        UserNameLabel.Text = Preferences.Get("UserName", "Kullanýcý");
        UserEmailLabel.Text = "BusGo üyesi";
    }

    private async void OnEditProfileTapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("EditProfilePage");

    private async void OnSettingsTapped(object sender, TappedEventArgs e)
        => await DisplayAlert("Ayarlar", "Ayarlar sayfasý daha sonra eklenecek.", "Tamam");

    private async void OnPersonalInfoTapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("EditProfilePage");

    private async void OnMyTripsTapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("//MyTripsPage");

    private async void OnFavoritesTapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("//FavoritesPage");

    private async void OnPaymentMethodsTapped(object sender, TappedEventArgs e)
        => await DisplayAlert("Ödeme Yöntemleri", "Ödeme yöntemleri sayfasý daha sonra eklenecek.", "Tamam");

    private async void OnNotificationsTapped(object sender, TappedEventArgs e)
        => await DisplayAlert("Bildirimler", "Bildirim tercihleri sayfasý daha sonra eklenecek.", "Tamam");

    private async void OnSupportTapped(object sender, TappedEventArgs e)
        => await DisplayAlert("Yardým ve Destek", "Destek sayfasý daha sonra eklenecek.", "Tamam");

    private async void OnLogoutTapped(object sender, TappedEventArgs e)
    {
        bool onay = await DisplayAlert("Çýkýþ Yap",
            "Hesabýnýzdan çýkýþ yapmak istediðinize emin misiniz?",
            "Evet, çýkýþ yap", "Vazgeç");

        if (!onay) return;

        Preferences.Remove("UserId");
        Preferences.Remove("UserName");
        Preferences.Remove("UserEmail");

        await Shell.Current.GoToAsync("//LoginPage");
    }
}