using BusGoMobile.Services;

namespace BusGoMobile.Pages;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _db;
    private bool _emailMode = true;   // true = e-posta, false = telefon
    public LoginPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    // E-posta sekmesi
    private void OnEmailTabTapped(object sender, TappedEventArgs e)
    {
        _emailMode = true;

        TabEmailBorder.BackgroundColor = (Color)Application.Current.Resources["BgSurfaceContainerLowest"];
        TabEmailLabel.FontFamily = "InterSemiBold";
        TabEmailLabel.TextColor = (Color)Application.Current.Resources["BgOnSurface"];

        TabPhoneBorder.BackgroundColor = Colors.Transparent;
        TabPhoneLabel.FontFamily = "InterRegular";
        TabPhoneLabel.TextColor = (Color)Application.Current.Resources["BgOnSurfaceVariant"];

        PrimaryInputLabel.Text = "E-POSTA ADRESÝ";
        PrimaryEntry.Placeholder = "ornek@email.com";
        PrimaryEntry.Keyboard = Keyboard.Email;
        PrimaryInputIcon.Text = (string)Application.Current.Resources["IconAlternateEmail"];
    }

    // Telefon sekmesi
    private void OnPhoneTabTapped(object sender, TappedEventArgs e)
    {
        _emailMode = false;

        TabPhoneBorder.BackgroundColor = (Color)Application.Current.Resources["BgSurfaceContainerLowest"];
        TabPhoneLabel.FontFamily = "InterSemiBold";
        TabPhoneLabel.TextColor = (Color)Application.Current.Resources["BgOnSurface"];

        TabEmailBorder.BackgroundColor = Colors.Transparent;
        TabEmailLabel.FontFamily = "InterRegular";
        TabEmailLabel.TextColor = (Color)Application.Current.Resources["BgOnSurfaceVariant"];

        PrimaryInputLabel.Text = "TELEFON NUMARASI";
        PrimaryEntry.Placeholder = "05XX XXX XX XX";
        PrimaryEntry.Keyboard = Keyboard.Telephone;
        PrimaryInputIcon.Text = (string)Application.Current.Resources["IconPhoneIphone"];
    }

    // Þifre göster/gizle
    private void OnTogglePasswordTapped(object sender, TappedEventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        PassEyeIcon.Text = PasswordEntry.IsPassword
            ? (string)Application.Current.Resources["IconVisibility"]
            : (string)Application.Current.Resources["IconVisibilityOff"];
    }

    // Giriþ
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PrimaryEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Eksik Bilgi", "Lütfen e-posta ve þifrenizi girin.", "Tamam");
            return;
        }

        if (!_emailMode)
        {
            await DisplayAlert("Bilgi", "Telefon ile giriþ henüz aktif deðil, lütfen e-posta ile giriþ yapýn.", "Tamam");
            return;
        }

        var user = await _db.GetUserAsync(PrimaryEntry.Text.Trim(), PasswordEntry.Text);

        if (user is null)
        {
            await DisplayAlert("Giriþ Baþarýsýz", "E-posta veya þifre hatalý.", "Tamam");
            return;
        }

        await DisplayAlert("Hoþ Geldiniz", $"Giriþ baþarýlý! Merhaba {user.FirstName}.", "Tamam");

        // Sonraki adýmda burada arama sayfasýna yönlendireceðiz (navigasyon).
    }

    // Kayýt ol sayfasýna git (navigasyon henüz kurulmadý)
    private async void OnGoToRegisterTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Bilgi", "Kayýt sayfasýna geçiþ navigasyon adýmýnda baðlanacak.", "Tamam");
    }
}