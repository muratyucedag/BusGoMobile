
namespace BusGoMobile.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int MarketingOptIn { get; set; }
        public string CreatedAt { get; set; }
        public int Points { get; set; }

    }
}
