using BusGoMobile.Models;
using BusGoMobile.Services;

namespace BusGoMobile.Pages;

public partial class HomePage : ContentPage
{
    private readonly DatabaseService _db;

    private List<Campaign> _kampanyalar = new();
    private int _aktifSlayt = 0;

    public HomePage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    // Sayfa her göründüğünde verileri yükle
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Giriş yapan kullanıcının adını göster
        string isim = Preferences.Get("UserName", "Kullanıcı");
        GreetingLabel.Text = $"Merhaba, {isim} 👋";

        if (CategoriesView.ItemsSource == null)
        {
            var kategoriler = await _db.GetCategoriesAsync();
            CategoriesView.ItemsSource = kategoriler;
        }

        if (_kampanyalar.Count == 0)
        {
            _kampanyalar = await _db.GetCampaignsAsync();
            KurSlider();
        }

        // Popüler rotalar (UI için statik)
        if (PopularRoutesView.ItemsSource == null)
        {
            var turuncu1 = (Color)Application.Current.Resources["BgSecondaryContainer"];
            var turuncu2 = (Color)Application.Current.Resources["BgSecondary"];
            var lacivert1 = (Color)Application.Current.Resources["BgPrimary"];
            var lacivert2 = (Color)Application.Current.Resources["BgInverseSurface"];

            PopularRoutesView.ItemsSource = new List<PopularRoute>
            {
                new() { Route = "İstanbul → Ankara",   Trips = "Günlük 48 sefer", Price = "450 TL", Color1 = turuncu1,  Color2 = turuncu2 },
                new() { Route = "İzmir → İstanbul",     Trips = "Günlük 36 sefer", Price = "550 TL", Color1 = lacivert1, Color2 = lacivert2 },
                new() { Route = "Bursa → Antalya",      Trips = "Günlük 22 sefer", Price = "620 TL", Color1 = turuncu1,  Color2 = turuncu2 },
                new() { Route = "Ankara → Eskişehir",   Trips = "Günlük 54 sefer", Price = "180 TL", Color1 = lacivert1, Color2 = lacivert2 },
            };
        }
    }

    // Slider'ı kur: noktaları oluştur, ilk slaytı göster, otomatik döndürmeyi başlat
    private void KurSlider()
    {
        if (_kampanyalar.Count == 0) return;

        DotsContainer.Children.Clear();
        for (int i = 0; i < _kampanyalar.Count; i++)
        {
            DotsContainer.Children.Add(new BoxView
            {
                WidthRequest = 8,
                HeightRequest = 8,
                CornerRadius = 4,
                Color = (Color)Application.Current.Resources["BgSurfaceContainerHigh"]
            });
        }

        _aktifSlayt = 0;
        SlaytiGoster(0);

        if (_kampanyalar.Count > 1)
        {
            Dispatcher.StartTimer(TimeSpan.FromSeconds(4), () =>
            {
                if (_kampanyalar.Count == 0)
                    return false;

                int sonraki = _aktifSlayt + 1;
                if (sonraki >= _kampanyalar.Count)
                    sonraki = 0;

                SlaytiGoster(sonraki);
                return true;
            });
        }
    }

    // Belirli bir slaytı ekrana yansıt
    private void SlaytiGoster(int index)
    {
        if (index < 0 || index >= _kampanyalar.Count) return;

        _aktifSlayt = index;
        var k = _kampanyalar[index];

        SlideImage.Source = k.ImageName;
        SlideTitle.Text = k.Title;
        SlideSubtitle.Text = k.Subtitle;

        var aktifRenk = (Color)Application.Current.Resources["BgSecondary"];
        var pasifRenk = (Color)Application.Current.Resources["BgSurfaceContainerHigh"];

        for (int i = 0; i < DotsContainer.Children.Count; i++)
        {
            if (DotsContainer.Children[i] is BoxView dot)
                dot.Color = (i == index) ? aktifRenk : pasifRenk;
        }
    }

    // Görsele dokununca bir sonraki slayta geç
    private void OnSlideTapped(object sender, TappedEventArgs e)
    {
        int sonraki = _aktifSlayt + 1;
        if (sonraki >= _kampanyalar.Count)
            sonraki = 0;
        SlaytiGoster(sonraki);
    }

    // Tümünü Gör
    private async void OnViewAllRoutesTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Popüler Rotalar", "Tüm rotalar sayfası daha sonra eklenecek.", "Tamam");
    }

    // Bildirim ikonu
    private async void OnNotificationsTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Bildirimler", "Henüz yeni bir bildiriminiz yok.", "Tamam");
    }

    // Profil ikonu
    private async void OnProfileTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//ProfilePage");
    }
}

// Popüler rotalar için minimal model (UI amaçlı, DB tablosu değil)
public class PopularRoute
{
    public string Route { get; set; }
    public string Trips { get; set; }
    public string Price { get; set; }
    public Color Color1 { get; set; }
    public Color Color2 { get; set; }
}