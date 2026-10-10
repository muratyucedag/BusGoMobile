namespace BusGoMobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("RegisterPage", typeof(Pages.RegisterPage));
            Routing.RegisterRoute("SearchPage", typeof(Pages.SearchPage));
            Routing.RegisterRoute("TripListPage", typeof(Pages.TripListPage));
            Routing.RegisterRoute("SeatSelectionPage", typeof(Pages.SeatSelectionPage));
            Routing.RegisterRoute("PassengerInfoPage", typeof(Pages.PassengerInfoPage));
            Routing.RegisterRoute("PaymentPage", typeof(Pages.PaymentPage));

        }
    }
}
