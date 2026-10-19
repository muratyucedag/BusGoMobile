using System.Text.RegularExpressions;
using BusGoMobile.Models;
using BusGoMobile.Services;

namespace BusGoMobile.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly DatabaseService _db;
    private bool _busy = false;

    public RegisterPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    // Focus: hangi input odaklanýrsa onun Border'ýný bul, turuncu yap
    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry && entry.Parent?.Parent is Border border)
            border.Stroke = (Color)Application.Current.Resources["BgSecondary"];
        else if (sender is Entry e2 && e2.Parent is Border b2)
            b2.Stroke = (Color)Application.Current.Resources["BgSecondary"];
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry && entry.Parent?.Parent is Border border)
            border.Stroke = (Color)Application.Current.Resources["BgOutlineVariant"];
        else if (sender is Entry e2 && e2.Parent is Border b2)
            b2.Stroke = (Color)Application.Current.Resources["BgOutlineVariant"];
    }

    // Þifre göster/gizle
    private void OnTogglePasswordTapped(object sender, TappedEventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        PassEyeIcon.Text = PasswordEntry.IsPassword
            ? (string)Application.Current.Resources["IconVisibility"]
            : (string)Application.Current.Resources["IconVisibilityOff"];
    }

    // Þifre gücü
    private void OnPasswordChanged(object sender, TextChangedEventArgs e)
    {
        string val = e.NewTextValue ?? "";

        var bos = (Color)Application.Current.Resources["BgSurfaceContainerHighest"];
        var zayif = (Color)Application.Current.Resources["BgMetro"];
        var orta = (Color)Application.Current.Resources["BgSecondary"];
        var guclu = Color.FromArgb("#16a34a");

        StrengthBar1.Color = bos;
        StrengthBar2.Color = bos;
        StrengthBar3.Color = bos;

        if (string.IsNullOrEmpty(val)) return;

        bool uzunluk = val.Length >= 8;
        bool harf = Regex.IsMatch(val, "[a-zA-ZðüþýöçÐÜÞÝÖÇ]");
        bool rakam = Regex.IsMatch(val, "[0-9]");

        int puan = 0;
        if (val.Length > 0) puan++;
        if (uzunluk && (harf || rakam)) puan++;
        if (uzunluk && harf && rakam) puan++;

        if (puan >= 1) StrengthBar1.Color = zayif;
        if (puan >= 2) { StrengthBar1.Color = orta; StrengthBar2.Color = orta; }
        if (puan >= 3) { StrengthBar1.Color = guclu; StrengthBar2.Color = guclu; StrengthBar3.Color = guclu; }
    }

    private async void OnRegisterClicked(object sender, TappedEventArgs e)
    {
        if (_busy) return;

        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) ||
            string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Eksik Bilgi", "Lütfen tüm zorunlu alanlarý doldurun.", "Tamam");
            return;
        }

        if (!TermsCheck.IsChecked)
        {
            await DisplayAlert("Onay Gerekli", "Kullaným koþullarýný kabul etmelisiniz.", "Tamam");
            return;
        }

        SetBusy(true);

        if (await _db.EmailExistsAsync(EmailEntry.Text.Trim()))
        {
            SetBusy(false);
            await DisplayAlert("Kayýtlý E-posta", "Bu e-posta ile zaten bir hesap var.", "Tamam");
            return;
        }

        var user = new User
        {
            FirstName = FirstNameEntry.Text.Trim(),
            LastName = LastNameEntry.Text.Trim(),
            Email = EmailEntry.Text.Trim(),
            Phone = PhoneEntry.Text?.Trim(),
            Password = PasswordEntry.Text,
            MarketingOptIn = MarketingCheck.IsChecked ? 1 : 0,
            CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        try
        {
            await _db.AddUserAsync(user);
            SetBusy(false);
            await DisplayAlert("Baþarýlý", "Hesabýnýz oluþturuldu! Giriþ yapabilirsiniz.", "Tamam");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            SetBusy(false);
            await DisplayAlert("Hata", "Kayýt sýrasýnda bir sorun oluþtu: " + ex.Message, "Tamam");
        }
    }

    private void SetBusy(bool busy)
    {
        _busy = busy;
        RegisterButtonContent.IsVisible = !busy;
        RegisterSpinner.IsVisible = busy;
        RegisterSpinner.IsRunning = busy;
        RegisterButtonBorder.Opacity = busy ? 0.85 : 1.0;
    }

    private async void OnGoToLoginTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnBackTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}