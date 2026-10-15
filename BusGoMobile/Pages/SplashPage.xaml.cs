namespace BusGoMobile.Pages;

public partial class SplashPage : ContentPage
{
    // Görseller + baþlýklar (istemezsen baþlýk/açýklamayý boþ býrakabilirsin)
    private readonly (string Image, string Title, string Subtitle)[] _slides =
    {
        ("splash1.jpg", "Binlerce sefer, tek yerde", "Türkiye'nin dört bir yanýna kolayca bilet bul."),
        ("splash2.jpg", "Keþfet ve yola çýk", "Yeni þehirler, yeni rotalar seni bekliyor."),
        ("splash3.jpg", "Güvenle yolculuk et", "Tüm firmalar tek uygulamada, güvenli ödeme."),
        ("splash4.jpg", "Koltuðunu sen seç", "Dilediðin koltuðu anýnda rezerve et."),
        ("splash5.jpg", "Yolculuðun keyfini çýkar", "Konforlu seferlerle rahat bir yolculuk."),
        ("splash6.jpg", "Her an yanýnda", "Biletlerin ve seferlerin cebinde."),
        ("splash7.jpg", "Avantajlý fiyatlar", "Kampanyalar ve indirimlerle uygun yolculuk."),
        ("splash8.jpg", "BusGo ile hazýrsýn", "Hadi ilk yolculuðunu planlayalým!"),
    };

    private int _index = 0;
    private const int SlideSeconds = 3;   // her görselin süresi
    private bool _bitti = false;

    public SplashPage()
    {
        InitializeComponent();
        KurNoktalar();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await SlaytDongusu();
    }

    private void KurNoktalar()
    {
        DotsContainer.Children.Clear();
        for (int i = 0; i < _slides.Length; i++)
        {
            DotsContainer.Children.Add(new BoxView
            {
                WidthRequest = 8,
                HeightRequest = 8,
                CornerRadius = 4,
                Color = Color.FromArgb("#50FFFFFF")
            });
        }
    }

    private async Task SlaytDongusu()
    {
        while (!_bitti && _index < _slides.Length)
        {
            await SlaytGoster(_index);
            _index++;
        }

        if (!_bitti)
            await AnaSayfayaGec();
    }

    private async Task SlaytGoster(int i)
    {
        var slide = _slides[i];

        // Görseli ve yazýlarý ayarla
        SplashImage.Source = slide.Image;
        SlideTitle.Text = slide.Title;
        SlideSubtitle.Text = slide.Subtitle;

        // Noktalarý güncelle
        for (int d = 0; d < DotsContainer.Children.Count; d++)
        {
            if (DotsContainer.Children[d] is BoxView dot)
                dot.Color = d == i
                    ? Colors.White
                    : Color.FromArgb("#50FFFFFF");
        }

        // Zoom efekti: görseli baþta normal boyuta al, sonra yavaþça büyüt
        SplashImage.Scale = 1.0;
        SlideTitle.Opacity = 0;
        SlideSubtitle.Opacity = 0;

        // Yazýlar yumuþak gelsin
        _ = SlideTitle.FadeTo(1, 500);
        _ = SlideSubtitle.FadeTo(0.85, 700);

        // Görsel süre boyunca yavaþça %115'e büyüsün (Ken Burns / zoom hissi)
        await SplashImage.ScaleTo(1.15, (uint)(SlideSeconds * 1000), Easing.Linear);
    }

    private async void OnSkipTapped(object sender, TappedEventArgs e)
    {
        await AnaSayfayaGec();
    }

    private async Task AnaSayfayaGec()
    {
        if (_bitti) return;
        _bitti = true;

        await Shell.Current.GoToAsync("//LoginPage");
    }
}