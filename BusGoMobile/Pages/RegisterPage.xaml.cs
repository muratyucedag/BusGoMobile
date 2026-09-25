using BusGoMobile.Models;
using BusGoMobile.Services;
using System.Text.RegularExpressions;

namespace BusGoMobile.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly DatabaseService _db;
    public RegisterPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }


    // Þifre göster/gizle
    private void OnTogglePasswordTapped(object sender, TappedEventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        PassEyeIcon.Text = PasswordEntry.IsPassword
            ? (string)Application.Current.Resources["IconVisibility"]
            : (string)Application.Current.Resources["IconVisibilityOff"];
    }

    // Þifre gücü çubuklarý
    private void OnPasswordChanged(object sender, TextChangedEventArgs e)
    {
        string val = e.NewTextValue ?? "";

        var bos = (Color)Application.Current.Resources["BgSurfaceContainerHighest"];
        var zayif = (Color)Application.Current.Resources["BgMetro"];      // kýrmýzý
        var orta = (Color)Application.Current.Resources["BgSecondary"];   // turuncu
        var guclu = Color.FromArgb("#16a34a");                            // yeþil

        // Önce hepsini sýfýrla
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

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) || string.IsNullOrWhiteSpace(LastNameEntry.Text) || string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Eksik Bilgi", "Lütfen tüm zorunlu alanlarý doldurun.", "Tamam");
            return;
        }

        if (!TermsCheck.IsChecked)
        {
            await DisplayAlert("Onay Gerekli", "Kullaným koþullarýný kabul etmelisiniz.", "Tamam");
            return;
        }

        if (await _db.EmailExistsAsync(EmailEntry.Text.Trim()))
        {
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
            await DisplayAlert("Baþarýlý", "Hesabýnýz oluþturuldu!", "Tamam");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", "Kayýt sýrasýnda bir sorun oluþtu: " + ex.Message, "Tamam");
        }
    }

}