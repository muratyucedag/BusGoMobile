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

    // Sayfa her göründüðünde verileri yükle
    protected override async void OnAppearing()
    {
        base.OnAppearing();

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
    }

    // Slider'ý kur: noktalarý oluþtur, ilk slaytý göster, otomatik döndürmeyi baþlat
    private void KurSlider()
    {
        if (_kampanyalar.Count == 0) return;

        // Noktalarý oluþtur
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

        // Otomatik döndürme
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

    // Belirli bir slaytý ekrana yansýt
    private void SlaytiGoster(int index)
    {
        if (index < 0 || index >= _kampanyalar.Count) return;

        _aktifSlayt = index;
        var k = _kampanyalar[index];

        SlideImage.Source = k.ImageName;
        SlideTitle.Text = k.Title;
        SlideSubtitle.Text = k.Subtitle;

        // Noktalarý güncelle: aktif olan turuncu, diðerleri gri
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
}