using BusGoMobile.Models;
using BusGoMobile.Services;

namespace BusGoMobile.Pages;

public partial class EditProfilePage : ContentPage
{
    private readonly DatabaseService _db;
    private User _user;
    private bool _busy;
    public EditProfilePage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        int userId = Preferences.Get("UserId", 0);
        if (userId == 0) return;

        _user = await _db.GetUserByIdAsync(userId);
        if (_user == null) return;

        FirstNameEntry.Text = _user.FirstName;
        LastNameEntry.Text = _user.LastName;
        EmailEntry.Text = _user.Email;
        PhoneEntry.Text = _user.Phone;

        UserNamePreviewLabel.Text = $"{_user.FirstName} {_user.LastName}";
    }

    // Focus: odaklanan input'un Border'ýný turuncu yap
    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry && entry.Parent?.Parent is Border border)
            border.Stroke = (Color)Application.Current.Resources["BgSecondary"];
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry && entry.Parent?.Parent is Border border)
            border.Stroke = (Color)Application.Current.Resources["BgOutlineVariant"];
    }

    private async void OnSaveTapped(object sender, TappedEventArgs e)
    {
        if (_busy || _user == null) return;

        // Validasyon
        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) ||
            string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text))
        {
            await DisplayAlert("Eksik Bilgi", "Ad, soyad ve e-posta boþ býrakýlamaz.", "Tamam");
            return;
        }

        string email = EmailEntry.Text.Trim();
        if (!email.Contains("@") || !email.Contains("."))
        {
            await DisplayAlert("Geçersiz E-posta", "Lütfen geçerli bir e-posta adresi girin.", "Tamam");
            return;
        }

        SetBusy(true);

        // E-posta baþka kullanýcýda var mý?
        if (await _db.EmailExistsForAnotherUserAsync(email, _user.Id))
        {
            SetBusy(false);
            await DisplayAlert("Kayýtlý E-posta", "Bu e-posta baþka bir hesapta kullanýlýyor.", "Tamam");
            return;
        }

        // Güncelle
        _user.FirstName = FirstNameEntry.Text.Trim();
        _user.LastName = LastNameEntry.Text.Trim();
        _user.Email = email;
        _user.Phone = PhoneEntry.Text?.Trim();

        try
        {
            await _db.UpdateUserAsync(_user);

            // Preferences'ý da güncelle
            Preferences.Set("UserName", _user.FirstName);
            Preferences.Set("UserEmail", _user.Email);

            SetBusy(false);
            await DisplayAlert("Profil Güncellendi", "Bilgileriniz baþarýyla güncellendi.", "Tamam");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            SetBusy(false);
            await DisplayAlert("Hata", "Güncelleme sýrasýnda bir sorun oluþtu: " + ex.Message, "Tamam");
        }
    }

    private void SetBusy(bool busy)
    {
        _busy = busy;
        SaveButtonContent.IsVisible = !busy;
        SaveSpinner.IsVisible = busy;
        SaveSpinner.IsRunning = busy;
        SaveButtonBorder.Opacity = busy ? 0.85 : 1.0;
    }

    private async void OnBackTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}