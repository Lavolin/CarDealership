using CarDealership.Core.Contracts;
using CarDealership.Core.Services;
using CarDealership.Infrastructure.Data;
using CarDealership.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

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

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
