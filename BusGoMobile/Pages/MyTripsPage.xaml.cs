namespace BusGoMobile.Pages;

public partial class MyTripsPage : ContentPage
{
	public MyTripsPage()
	{
		InitializeComponent();
	}

    private void OnActiveTabTapped(object sender, TappedEventArgs e)
    {
        bool aktifGoruniyor = ActiveView.IsVisible;

        ActiveView.IsVisible = !aktifGoruniyor;
        PastView.IsVisible = aktifGoruniyor;

        var secili = (Color)Application.Current.Resources["BgSecondary"];
        var soluk = (Color)Application.Current.Resources["BgOnSurfaceVariant"];

        if (ActiveView.IsVisible)
        {
            TabActiveLabel.TextColor = secili;
            TabActiveLabel.FontFamily = "InterBold";
            TabActiveIndicator.Color = secili;

            TabPastLabel.TextColor = soluk;
            TabPastLabel.FontFamily = "InterRegular";
            TabPastIndicator.Color = Colors.Transparent;
        }
        else
        {
            TabPastLabel.TextColor = secili;
            TabPastLabel.FontFamily = "InterBold";
            TabPastIndicator.Color = secili;

            TabActiveLabel.TextColor = soluk;
            TabActiveLabel.FontFamily = "InterRegular";
            TabActiveIndicator.Color = Colors.Transparent;
        }
    }
}