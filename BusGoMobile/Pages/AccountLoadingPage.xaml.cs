namespace BusGoMobile.Pages;

public partial class AccountLoadingPage : ContentPage
{
    private bool _isNavigating;

    public AccountLoadingPage()
    {
        InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isNavigating) return;
        _isNavigating = true;

        // Öðeler yumuþak belirsin
        await BrandRow.FadeTo(1, 400);
        _ = LoaderArea.FadeTo(1, 400);
        _ = TextArea.FadeTo(1, 500);

        // 3 saniye bekle
        await Task.Delay(5000);

        // Ana sayfaya geç
        await Shell.Current.GoToAsync("//LoginSuccessPage");
    }
}