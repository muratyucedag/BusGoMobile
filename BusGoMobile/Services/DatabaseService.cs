using BusGoMobile.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusGoMobile.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _db;

        private const string DbFileName = "BusGoDb.db";

        // Bağlantıyı hazırla: gömülü dosyayı yazılabilir klasöre kopyala, sonra bağlan
        private async Task InitAsync()
        {
            if (_db is not null)
                return;

            // GEÇİCİ: Geliştirme için masaüstündeki sabit dosyaya bağlan.
            // İleride proje içine gömülü dosyaya geçilecek.
            string targetPath = @"C:\Users\Murat\Desktop\BusGoDb.db";

            _db = new SQLiteAsyncConnection(targetPath);

            System.Diagnostics.Debug.WriteLine($"VERITABANI YOLU: {targetPath}");
        }

        // Yeni kullanıcı ekle (ham INSERT query'si)
        public async Task<int> AddUserAsync(User user)
        {
            await InitAsync();

            return await _db.ExecuteAsync(
                @"INSERT INTO User (FirstName, LastName, Email, Phone, Password, MarketingOptIn, CreatedAt)
              VALUES (?, ?, ?, ?, ?, ?, ?);",
                user.FirstName, user.LastName, user.Email, user.Phone,
                user.Password, user.MarketingOptIn, user.CreatedAt);
        }

        // E-posta zaten kayıtlı mı?
        public async Task<bool> EmailExistsAsync(string email)
        {
            await InitAsync();

            int count = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM User WHERE Email = ?;", email);

            return count > 0;
        }

        // Login için: e-posta + şifre ile kullanıcı bul
        public async Task<User> GetUserAsync(string email, string password)
        {
            await InitAsync();

            var results = await _db.QueryAsync<User>("SELECT * FROM User WHERE Email = ? AND Password = ? LIMIT 1;", email, password);

            return results.FirstOrDefault();
        }

        // Tüm kategorileri getir
        public async Task<List<Category>> GetCategoriesAsync()
        {
            await InitAsync();

            return await _db.QueryAsync<Category>("SELECT * FROM Category ORDER BY Id;");
        }

        // Tüm şehirleri getir
        public async Task<List<City>> GetCitiesAsync()
        {
            await InitAsync();

            return await _db.QueryAsync<City>("SELECT * FROM City ORDER BY Name;");
        }

        //Slider Verilerini Ana Sayfaya Getir
        public async Task<List<Campaign>> GetCampaignsAsync()
        {
            await InitAsync();
            return await _db.QueryAsync<Campaign>("SELECT * FROM Campaign ORDER BY Id;");
        }

        // Belirli bir tarihteki seferleri getir (fiyata göre sıralı)
        public async Task<List<Trip>> GetTripsAsync(string travelDate)
        {
            await InitAsync();

            return await _db.QueryAsync<Trip>(
                "SELECT * FROM Trip WHERE TravelDate = ? ORDER BY Price;",
                travelDate);
        }

        // Tüm seferleri getir (test için)
        public async Task<List<Trip>> GetAllTripsAsync()
        {
            await InitAsync();
            return await _db.QueryAsync<Trip>("SELECT * FROM Trip ORDER BY Price;");
        }

        // Tüm seferleri, her birinin olanaklarıyla birlikte getir
        public async Task<List<Trip>> GetAllTripsWithAmenitiesAsync()
        {
            await InitAsync();

            // 1) Seferleri çek (fiyata göre sıralı)
            var trips = await _db.QueryAsync<Trip>("SELECT * FROM Trip ORDER BY Price;");

            if (trips.Count == 0)
                return trips;

            // 2) TÜM sefer-olanak eşleşmelerini tek sorguda çek (JOIN)
            var rows = await _db.QueryAsync<TripAmenityRow>(
                @"SELECT ta.TripId AS TripId, a.Name AS Name, a.IconCode AS IconCode
              FROM TripAmenity ta
              JOIN Amenity a ON a.Id = ta.AmenityId;");

            // 3) Eşleşmeleri seferlere dağıt
            foreach (var trip in trips)
            {
                trip.AmenityList = rows
                    .Where(r => r.TripId == trip.Id)
                    .Select(r => new Amenity { Name = r.Name, IconCode = r.IconCode })
                    .ToList();
            }

            // 4) En ucuz sefere işaret koy
            trips[0].IsCheapest = true;

            return trips;
        }
    }
}
