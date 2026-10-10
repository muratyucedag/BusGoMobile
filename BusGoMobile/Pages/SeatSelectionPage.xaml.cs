using BusGoMobile.Models;
using BusGoMobile.Services;

namespace BusGoMobile.Pages;

[QueryProperty(nameof(TripId), "tripId")]
public partial class SeatSelectionPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly List<Seat> _seats = new();
    private Seat _selectedSeat;
    private const int BasePrice = 520;

    public int TripId { get; set; }

    public SeatSelectionPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_seats.Count == 0)
            await KoltuklariYukle();
    }

    private async Task KoltuklariYukle()
    {
        var dbSeats = await _db.GetSeatsAsync(TripId);

        _seats.Clear();
        _seats.AddRange(dbSeats);

        CizKoltuklar();
        OzetGuncelle();
    }

    // Koltuk tipi (sol tek / sağ pencere / sağ koridor / arka)
    private string KoltukTipi(int number)
    {
        if (number >= 25) return "Arka Koltuk";

        int siraIcindeki = (number - 1) % 3;
        return siraIcindeki switch
        {
            0 => "Tekli Koltuk",
            1 => "Çiftli (Pencere)",
            2 => "Çiftli (Koridor)",
            _ => "Koltuk"
        };
    }

    // 2+1 çiftteki komşu koltuk (cinsiyet kuralı için)
    private Seat CiftKomsu(int number)
    {
        if (number >= 25) return null;

        int siraIcindeki = (number - 1) % 3;
        if (siraIcindeki == 1) return _seats.FirstOrDefault(s => s.Number == number + 1);
        if (siraIcindeki == 2) return _seats.FirstOrDefault(s => s.Number == number - 1);
        return null;
    }

    private void CizKoltuklar()
    {
        SeatsContainer.Children.Clear();

        if (_seats.Count == 0)
        {
            SeatsContainer.Children.Add(new Label
            {
                Text = "Bu sefer için koltuk bilgisi bulunamadı.",
                FontFamily = "InterRegular",
                FontSize = 13,
                TextColor = (Color)Application.Current.Resources["BgOnSurfaceVariant"],
                HorizontalOptions = LayoutOptions.Center
            });
            return;
        }

        int index = 0;
        int siraSayaci = 0;

        // Ön kısım: ilk 24 koltuk, 8 sıra 2+1
        while (index < 24 && index < _seats.Count)
        {
            var rowGrid = YeniSira();

            var sol = KoltukGorseli(_seats[index]);
            Grid.SetColumn(sol, 0);
            rowGrid.Children.Add(sol);
            index++;

            if (index < _seats.Count)
            {
                var sag1 = KoltukGorseli(_seats[index]);
                Grid.SetColumn(sag1, 2);
                rowGrid.Children.Add(sag1);
                index++;
            }

            if (index < _seats.Count)
            {
                var sag2 = KoltukGorseli(_seats[index]);
                Grid.SetColumn(sag2, 3);
                rowGrid.Children.Add(sag2);
                index++;
            }

            SeatsContainer.Children.Add(rowGrid);
            siraSayaci++;

            if (siraSayaci == 4)
                SeatsContainer.Children.Add(OrtaKapiBandi());
        }

        // Arka sıradan önce bilgi bandı
        SeatsContainer.Children.Add(BilgiBandi());

        // Arka 4'lü başlık
        SeatsContainer.Children.Add(new Label
        {
            Text = "ARKA 4'LÜ KOLTUK SIRASI",
            FontFamily = "InterBold",
            FontSize = 11,
            TextColor = (Color)Application.Current.Resources["BgOnSurfaceVariant"],
            HorizontalOptions = LayoutOptions.Center
        });

        // Arka 4'lü (25-28)
        var arkaGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            },
            ColumnSpacing = 8
        };
        int arkaSutun = 0;
        while (index < _seats.Count)
        {
            var koltuk = KoltukGorseli(_seats[index]);
            Grid.SetColumn(koltuk, arkaSutun);
            arkaGrid.Children.Add(koltuk);
            index++;
            arkaSutun++;
        }
        SeatsContainer.Children.Add(arkaGrid);
    }

    private Grid YeniSira() => new Grid
    {
        ColumnDefinitions =
        {
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
        },
        ColumnSpacing = 8
    };

    private View OrtaKapiBandi()
    {
        var border = new Border
        {
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 8 },
            BackgroundColor = (Color)Application.Current.Resources["BgSurfaceContainer"],
            Padding = new Thickness(10, 6)
        };
        border.Content = new Label
        {
            Text = "ORTA KAPI & ACİL ÇIKIŞ",
            FontFamily = "InterBold",
            FontSize = 11,
            TextColor = (Color)Application.Current.Resources["BgSecondary"],
            HorizontalOptions = LayoutOptions.Center
        };
        return border;
    }

    private View BilgiBandi()
    {
        var border = new Border
        {
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 8 },
            BackgroundColor = (Color)Application.Current.Resources["BgSurfaceContainer"],
            Padding = new Thickness(10, 6)
        };
        border.Content = new Label
        {
            Text = "İkram & WC  •  Geniş Diz Mesafesi",
            FontFamily = "InterMedium",
            FontSize = 11,
            TextColor = (Color)Application.Current.Resources["BgOnSurfaceVariant"],
            HorizontalOptions = LayoutOptions.Center
        };
        return border;
    }

    private View KoltukGorseli(Seat seat)
    {
        var border = new Border
        {
            HeightRequest = 46,
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 10 },
            BackgroundColor = KoltukRengi(seat)
        };

        var content = new VerticalStackLayout
        {
            Spacing = 0,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        content.Children.Add(new Label
        {
            Text = seat.Number.ToString(),
            FontFamily = "InterBold",
            FontSize = 12,
            TextColor = KoltukYaziRengi(seat),
            HorizontalOptions = LayoutOptions.Center
        });

        if (seat.StatusEnum == SeatStatus.Male || seat.StatusEnum == SeatStatus.Female)
        {
            var iconKey = seat.StatusEnum == SeatStatus.Male ? "IconMan" : "IconWoman";
            content.Children.Add(new Label
            {
                Text = (string)Application.Current.Resources[iconKey],
                FontFamily = "MaterialSymbols",
                FontSize = 13,
                TextColor = KoltukYaziRengi(seat),
                HorizontalOptions = LayoutOptions.Center
            });
        }
        else if (seat.StatusEnum == SeatStatus.Selected)
        {
            content.Children.Add(new Label
            {
                Text = (string)Application.Current.Resources["IconCheck"],
                FontFamily = "MaterialSymbols",
                FontSize = 13,
                TextColor = KoltukYaziRengi(seat),
                HorizontalOptions = LayoutOptions.Center
            });
        }

        border.Content = content;

        // Her koltuğa tıklama (dolu olana da — kişiyi göstermek için)
        var tap = new TapGestureRecognizer();
        tap.Tapped += (s, e) => KoltugaTiklandi(seat);
        border.GestureRecognizers.Add(tap);

        return border;
    }

    private Color KoltukRengi(Seat seat) => seat.StatusEnum switch
    {
        SeatStatus.Selected => (Color)Application.Current.Resources["BgSecondaryContainer"],
        SeatStatus.Male     => (Color)Application.Current.Resources["BgSurfaceContainerHighest"],
        SeatStatus.Female   => (Color)Application.Current.Resources["BgErrorContainer"],
        _                   => (Color)Application.Current.Resources["BgSurfaceContainerLow"]
    };

    private Color KoltukYaziRengi(Seat seat) => seat.StatusEnum switch
    {
        SeatStatus.Selected => (Color)Application.Current.Resources["BgOnSecondary"],
        SeatStatus.Male     => (Color)Application.Current.Resources["BgPrimary"],
        SeatStatus.Female   => (Color)Application.Current.Resources["BgError"],
        _                   => (Color)Application.Current.Resources["BgOnSurface"]
    };

    private async void KoltugaTiklandi(Seat seat)
    {
        // Dolu koltuk: kişiyi göster, seçime izin verme
        if (seat.StatusEnum == SeatStatus.Male || seat.StatusEnum == SeatStatus.Female)
        {
            string cinsiyet = seat.StatusEnum == SeatStatus.Female ? "Kadın" : "Erkek";
            string isim = string.IsNullOrWhiteSpace(seat.PassengerName) ? "Bilinmiyor" : seat.PassengerName;

            await DisplayAlert($"Koltuk {seat.Number}",
                $"Yolcu: {isim}\nCinsiyet: {cinsiyet}",
                "Kapat");
            return;
        }

        // Boş koltuk: yanı dolu çift koltuk mu? (cinsiyet kuralı uyarısı)
        if (seat.StatusEnum == SeatStatus.Empty)
        {
            var komsu = CiftKomsu(seat.Number);
            if (komsu != null && (komsu.StatusEnum == SeatStatus.Male || komsu.StatusEnum == SeatStatus.Female))
            {
                string cinsiyet = komsu.StatusEnum == SeatStatus.Female ? "kadın" : "erkek";
                await DisplayAlert("Cinsiyet Kuralı",
                    $"Yanınızdaki {komsu.Number} numaralı koltukta {cinsiyet} yolcu var. Bu koltuk yalnızca {cinsiyet} yolcu içindir.",
                    "Anladım");
            }
        }

        // Seçim mantığı
        if (_selectedSeat == seat)
        {
            seat.StatusEnum = SeatStatus.Empty;
            _selectedSeat = null;
        }
        else
        {
            if (_selectedSeat != null)
                _selectedSeat.StatusEnum = SeatStatus.Empty;

            seat.StatusEnum = SeatStatus.Selected;
            _selectedSeat = seat;
        }

        CizKoltuklar();
        OzetGuncelle();
    }

    private void OzetGuncelle()
    {
        int bos = _seats.Count(s => s.StatusEnum == SeatStatus.Empty);
        RemainingSeatsLabel.Text = $"{bos} boş koltuk";

        if (_selectedSeat == null)
        {
            SelectedSeatLabel.Text = "Koltuk Seçiniz";
            TotalPriceLabel.Text = "0 TL";
        }
        else
        {
            SelectedSeatLabel.Text = $"No: {_selectedSeat.Number} ({KoltukTipi(_selectedSeat.Number)})";
            TotalPriceLabel.Text = $"{BasePrice} TL";
        }
    }

    private async void OnProceedClicked(object sender, EventArgs e)
    {
        if (_selectedSeat == null)
        {
            await DisplayAlert("Koltuk Seçimi", "Lütfen devam etmek için bir koltuk seçin.", "Tamam");
            return;
        }

        // Yolcu bilgileri sayfasına git: sefer + koltuk numarasını taşı
        await Shell.Current.GoToAsync(
            $"PassengerInfoPage?tripId={TripId}&seatNumber={_selectedSeat.Number}");
    }
}