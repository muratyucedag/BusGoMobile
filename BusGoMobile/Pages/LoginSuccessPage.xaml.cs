namespace BusGoMobile.Pages;

public partial class LoginSuccessPage : ContentPage
{
    private bool _isNavigating;

    public LoginSuccessPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Giriþ animasyonu: check büyüyerek gelsin, metin ve buton fade
        _ = CheckCircle.FadeTo(1, 300);
        await CheckCircle.ScaleTo(1, 400, Easing.SpringOut);
        _ = TextArea.FadeTo(1, 400);
        await Task.Delay(300);
        _ = ContinueButton.FadeTo(1, 400);

        // Otomatik geçiþ (~2 sn), ama buton da görünür kalýr
        await Task.Delay(8000);
        await GoHome();
    }

    private async void OnContinueTapped(object sender, TappedEventArgs e)
    {
        await GoHome();
    }

    private async Task GoHome()
    {
        if (_isNavigating) return;
        _isNavigating = true;

        await Shell.Current.GoToAsync("//HomePage");
    }
}