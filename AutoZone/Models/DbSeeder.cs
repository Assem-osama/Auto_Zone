using Microsoft.EntityFrameworkCore;
using static AutoZone.Models.Enum;

namespace AutoZone.Models
{
    public class DbSeeder
    {
        public static void SeedData(AutoZoneDbContext db)
        {
            db.Database.Migrate();

            // 🧑‍💼 Seed Users
            if (!db.Users.Any())
            {
                var users = new List<User>
        {
            new User { Name = "Admin One", Email = "admin1@autozone.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"), Phone = "0100000001", Role = UserRole.Admin },
            new User { Name = "Admin Two", Email = "admin2@autozone.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin456"), Phone = "0100000002", Role = UserRole.Admin },
            new User { Name = "Assem", Email = "assem@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Assem123"), Phone = "0101234567", Role = UserRole.User },
            new User { Name = "Omar", Email = "omar@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Omar123"), Phone = "0109876543", Role = UserRole.User },
            new User { Name = "Sara", Email = "sara@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Sara123"), Phone = "0112233445", Role = UserRole.User }
        };

                db.Users.AddRange(users);
                db.SaveChanges();
            }

            var usersList = db.Users.ToList();

            // 🚗 Seed Cars
            if (!db.Cars.Any())
            {
                var assem = usersList.FirstOrDefault(u => u.Email == "assem@example.com");
                var omar = usersList.FirstOrDefault(u => u.Email == "omar@example.com");
                var sara = usersList.FirstOrDefault(u => u.Email == "sara@example.com");

                if (assem != null && omar != null && sara != null)
                {
                    var cars = new List<Car>
            {
                new Car { Brand = "Toyota", Model = "Corolla", Year = 2020, Price = 500000, PricePerDay = 800, status = CarStatus.ForSale, ImageUrl = "https://cdn.pixabay.com/photo/2017/01/06/19/15/toyota-1957037_1280.jpg", Description = "Reliable and fuel-efficient sedan in excellent condition.", UserId = assem.Id },
                new Car { Brand = "BMW", Model = "X5", Year = 2022, Price = 2500000, status = CarStatus.ForSale, ImageUrl = "https://cdn.pixabay.com/photo/2016/03/27/21/16/bmw-1281649_1280.jpg", Description = "Luxury SUV with premium features and low mileage.", UserId = omar.Id },
                new Car { Brand = "Kia", Model = "Sportage", Year = 2021, PricePerDay = 900, status = CarStatus.ForRent, ImageUrl = "https://cdn.pixabay.com/photo/2018/04/17/16/48/kia-3328158_1280.jpg", Description = "Spacious and comfortable SUV for rent, ideal for family trips.", UserId = sara.Id },
                new Car { Brand = "Mercedes", Model = "C200", Year = 2023, Price = 2800000, status = CarStatus.ForSale, ImageUrl = "https://cdn.pixabay.com/photo/2018/03/04/19/55/mercedes-3197227_1280.jpg", Description = "Brand new Mercedes with AMG package.", UserId = assem.Id },
                new Car { Brand = "Hyundai", Model = "Elantra", Year = 2019, PricePerDay = 600, status = CarStatus.ForRent, ImageUrl = "https://cdn.pixabay.com/photo/2021/06/23/07/34/hyundai-6358632_1280.jpg", Description = "Comfortable sedan for rent with great fuel economy.", UserId = omar.Id },
                new Car { Brand = "Audi", Model = "A6", Year = 2021, Price = 2000000, status = CarStatus.ForSale, ImageUrl = "https://cdn.pixabay.com/photo/2016/04/24/00/38/audi-1348113_1280.jpg", Description = "German luxury car, very clean and fully loaded.", UserId = sara.Id }
            };

                    db.Cars.AddRange(cars);
                    db.SaveChanges();
                }
            }

            var carsList = db.Cars.ToList();

            // 📅 Seed Rentals
            if (!db.Rentals.Any())
            {
                var sportage = carsList.FirstOrDefault(c => c.Model == "Sportage");
                var elantra = carsList.FirstOrDefault(c => c.Model == "Elantra");
                var assem = usersList.FirstOrDefault(u => u.Email == "assem@example.com");
                var sara = usersList.FirstOrDefault(u => u.Email == "sara@example.com");

                var rentals = new List<Rental>();

                if (sportage != null && assem != null)
                {
                    rentals.Add(new Rental
                    {
                        CarId = sportage.Id,
                        RenterId = assem.Id,
                        StartDate = new DateTime(2025, 10, 10),
                        EndDate = new DateTime(2025, 10, 15),
                        TotalPrice = 4500
                    });
                }

                if (elantra != null && sara != null)
                {
                    rentals.Add(new Rental
                    {
                        CarId = elantra.Id,
                        RenterId = sara.Id,
                        StartDate = new DateTime(2025, 9, 20),
                        EndDate = new DateTime(2025, 9, 25),
                        TotalPrice = 3000
                    });
                }

                db.Rentals.AddRange(rentals);
                db.SaveChanges();
            }

        }
    }
}
