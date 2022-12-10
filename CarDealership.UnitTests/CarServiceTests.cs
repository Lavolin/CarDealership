using CarDealership.Core.Contracts;
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
        public void Test1()
        {
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
