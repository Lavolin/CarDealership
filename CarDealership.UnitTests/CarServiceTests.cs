using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Car;
using CarDealership.Core.Services;
using CarDealership.Infrastructure.Data;
using CarDealership.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CarDealership.UnitTests
{
    [TestFixture]
    public class CarServiceTests
    {
        private IRepository repo;
        private ICarService carService;
        private ApplicationDbContext context;


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase("CarDealershipDB")
               .Options;

            context = new ApplicationDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task TestAll()
        {
            var repo = new Repository(context);
            carService = new CarService(repo);

            await repo.AddRangeAsync(new List<Car>()
            {
                new Car { Id = 1, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 2, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 3, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 4, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 5, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 6, Model = "", Description = "", ImageUrl = ""},

            });

            await repo.SaveChangesAsync();

            var result = await carService.All();

           
            Assert.That(3, Is.EqualTo(result.Cars.Count()));
        }

        [Test]
        public async Task AllCarsByDealerId()
        {
            var repo = new Repository(context);
            carService = new CarService(repo);

            await repo.AddRangeAsync(new List<Car>()
            {
                new Car { Id = 1, Model = "", Description = "", ImageUrl = "", DealerId = 1, IsActive = true},
                new Car { Id = 2, Model = "", Description = "", ImageUrl = "", DealerId = 1, IsActive = true},
                new Car { Id = 3, Model = "", Description = "", ImageUrl = "", DealerId = 2, IsActive = true}           
                

            });

            await repo.SaveChangesAsync();

            var result = await carService.AllCarsByDealerId(1);

            Assert.That(2, Is.EqualTo(result.Count()));            
        }

        [Test]
        public async Task AllCarsByUserId()
        {
            var repo = new Repository(context);
            carService = new CarService(repo);

            await repo.AddRangeAsync(new List<Car>()
            {
                new Car { Id = 1, Model = "", Description = "", ImageUrl = "", BuyerId = "1", IsActive = true},
                new Car { Id = 2, Model = "", Description = "", ImageUrl = "", BuyerId = "1", IsActive = true},
                new Car { Id = 3, Model = "", Description = "", ImageUrl = "", BuyerId = "2", IsActive = true}


            });

            await repo.SaveChangesAsync();

            var result = await carService.AllCarsByUserId("1");
            Assert.That(2, Is.EqualTo(result.Count()));

        }

        [Test]
        public async Task TestAllCategories()
        {
            var repo = new Repository(context);
            carService = new CarService(repo);
            await repo.AddRangeAsync(new List<CarCategory>()
            {
                new CarCategory { Id =1, Name = "1"}
            });

            await repo.SaveChangesAsync();

            var result = await carService.AllCategories();
            Assert.That(1, Is.EqualTo(result.Count()));

            var resultCatNames = await carService.AllCategoriesNames();
            Assert.That(resultCatNames, Is.Not.Null);

        }

        [Test]
        public async Task TestCategoryExists()
        {
            var repo = new Repository(context);
            carService = new CarService(repo);
            await repo.AddRangeAsync(new List<Car>()
            {
                new Car { Id = 1, Model = "", Description = "", ImageUrl = "", DealerId = 1, IsActive = true},
                new Car { Id = 2, Model = "", Description = "", ImageUrl = "", DealerId = 1, IsActive = true},
                new Car { Id = 3, Model = "", Description = "", ImageUrl = "", DealerId = 2, IsActive = true}


            });
            await repo.SaveChangesAsync();

            await repo.AddRangeAsync(new List<CarCategory>()
            {
                new CarCategory { Id =1, Name = "1"}
            });

            await repo.SaveChangesAsync();

            var catId = await carService.GetCarCategoryId(1);
            var result = await carService.CategoryExists(catId);

            Assert.That(result, Is.False);
        }

       

        [Test]
        public async Task TestLastFive()
        {
            var repo = new Repository(context);
            carService = new CarService(repo);

            await repo.AddRangeAsync(new List<Car>()
            {
                new Car { Id = 1, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 2, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 3, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 4, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 5, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 6, Model = "", Description = "", ImageUrl = ""},

            });

            await repo.SaveChangesAsync();

            var result = await carService.LastFiveCars();
           
           Assert.That(5, Is.EqualTo(result.Count()));
           Assert.That(result.Any(c => c.Id == 1), Is.False);

        }

        [Test]
        public async Task TestExists_IsBought()
        {
            var repo = new Repository(context);
            carService = new CarService(repo);

            await repo.AddRangeAsync(new List<Car>()
            {
                new Car { Id = 1, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 2, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 3, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 4, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 5, Model = "", Description = "", ImageUrl = ""},
                new Car { Id = 6, Model = "", Description = "", ImageUrl = ""},

            });

            await repo.SaveChangesAsync();

            var result = await carService.Exists(1);

            Assert.That(result, Is.True);

            var result1 = await carService.Exists(7);
            Assert.That(result1, Is.False);

            var result3 = await carService.IsBought(2);

            Assert.That(result3, Is.False);
        }

        [Test]
       

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
