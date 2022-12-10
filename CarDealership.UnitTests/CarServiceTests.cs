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
        public async Task TestCategory()
        {
            var repo = new Repository(context);
            carService = new CarService(repo);

            await repo.AddRangeAsync(new List<Car>()
            {
                new Car { Id = 1, Model = "", Description = "", ImageUrl = ""}
            });

            await repo.SaveChangesAsync();

            var result = await carService.LastFiveCars();
           
           Assert.That(1, Is.EqualTo(result.Count()));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
