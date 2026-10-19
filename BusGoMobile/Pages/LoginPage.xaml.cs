using BusGoMobile.Services;

namespace BusGoMobile.Pages;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _db;
    private bool _busy = false;

    public LoginPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    // --- Focus durumları: turuncu vurgu ---
    private void OnEmailFocused(object sender, FocusEventArgs e)
    {
        EmailBorder.Stroke = (Color)Application.Current.Resources["BgSecondary"];
        EmailIcon.TextColor = (Color)Application.Current.Resources["BgSecondary"];
    }

    private void OnEmailUnfocused(object sender, FocusEventArgs e)
    {
        EmailBorder.Stroke = (Color)Application.Current.Resources["BgOutlineVariant"];
        EmailIcon.TextColor = (Color)Application.Current.Resources["BgOutline"];
    }

    private void OnPasswordFocused(object sender, FocusEventArgs e)
    {
        PasswordBorder.Stroke = (Color)Application.Current.Resources["BgSecondary"];
        PasswordIcon.TextColor = (Color)Application.Current.Resources["BgSecondary"];
    }

    private void OnPasswordUnfocused(object sender, FocusEventArgs e)
    {
        PasswordBorder.Stroke = (Color)Application.Current.Resources["BgOutlineVariant"];
        PasswordIcon.TextColor = (Color)Application.Current.Resources["BgOutline"];
    }

    // --- Şifre göster/gizle ---
    private void OnTogglePasswordTapped(object sender, TappedEventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        PassEyeIcon.Text = PasswordEntry.IsPassword
            ? (string)Application.Current.Resources["IconVisibility"]
            : (string)Application.Current.Resources["IconVisibilityOff"];
    }

    // --- Giriş ---
    private async void OnLoginClicked(object sender, TappedEventArgs e)
    {
        if (_busy) return;

        if (string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Eksik Bilgi", "Lütfen e-posta ve şifrenizi girin.", "Tamam");
            return;
        }

        SetBusy(true);

        var user = await _db.GetUserAsync(EmailEntry.Text.Trim(), PasswordEntry.Text);

        SetBusy(false);

        if (user is null)
        {
            await DisplayAlert("Giriş Başarısız", "E-posta veya şifre hatalı.", "Tamam");
            return;
        }

        // Oturum bilgisini kaydet
        Preferences.Set("UserId", user.Id);
        Preferences.Set("UserName", user.FirstName);

        await Shell.Current.GoToAsync("//HomePage");
    }

    private void SetBusy(bool busy)
    {
        _busy = busy;
        LoginButtonContent.IsVisible = !busy;
        LoginSpinner.IsVisible = busy;
        LoginSpinner.IsRunning = busy;
        LoginButtonBorder.Opacity = busy ? 0.85 : 1.0;
    }

    private async void OnGoToRegisterTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("RegisterPage");
    }
}