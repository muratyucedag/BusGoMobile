namespace BusGoMobile.Controls;

public partial class BottomNavBar : ContentView
{
    // Hangi sekmede olduðumuzu dýþarýdan alacaðýz (örn. "home", "trips", "fav", "profile")
    public static readonly BindableProperty ActiveTabProperty =
        BindableProperty.Create(nameof(ActiveTab), typeof(string), typeof(BottomNavBar),
            default(string), propertyChanged: OnActiveTabChanged);

    public string ActiveTab
    {
        get => (string)GetValue(ActiveTabProperty);
        set => SetValue(ActiveTabProperty, value);
    }

    public BottomNavBar()
    {
        InitializeComponent();
    }

    // Aktif sekme deðiþince, o sekmeyi vurgula (turuncu yap)
    private static void OnActiveTabChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var bar = (BottomNavBar)bindable;
        bar.Highlight((string)newValue);
    }

    private void Highlight(string tab)
    {
        var aktif = (Color)Application.Current.Resources["BgSecondary"];
        var pasif = (Color)Application.Current.Resources["BgOnSurfaceVariant"];

        // Önce hepsini pasif yap
        HomeIcon.TextColor = pasif; HomeText.TextColor = pasif;
        TripsIcon.TextColor = pasif; TripsText.TextColor = pasif;
        FavIcon.TextColor = pasif; FavText.TextColor = pasif;
        ProfileIcon.TextColor = pasif; ProfileText.TextColor = pasif;

        // Aktif olaný vurgula
        switch (tab)
        {
            case "home":
                HomeIcon.TextColor = aktif; HomeText.TextColor = aktif;
                break;
            case "trips":
                TripsIcon.TextColor = aktif; TripsText.TextColor = aktif;
                break;
            case "fav":
                FavIcon.TextColor = aktif; FavText.TextColor = aktif;
                break;
            case "profile":
                ProfileIcon.TextColor = aktif; ProfileText.TextColor = aktif;
                break;
        }
    }

    // Navigasyon: her butona basýnca ilgili sayfaya git
    private async void OnHomeTapped(object sender, TappedEventArgs e)
    {
        if (ActiveTab != "home")
            await Shell.Current.GoToAsync("//HomePage");
    }

    private async void OnTripsTapped(object sender, TappedEventArgs e)
    {
        if (ActiveTab != "trips")
            await Shell.Current.GoToAsync("//MyTripsPage");
    }

    private async void OnFavTapped(object sender, TappedEventArgs e)
    {
        if (ActiveTab != "fav")
            await Shell.Current.GoToAsync("//FavoritesPage");
    }

    private async void OnProfileTapped(object sender, TappedEventArgs e)
    {
        if (ActiveTab != "profile")
            await Shell.Current.GoToAsync("//ProfilePage");
    }
}