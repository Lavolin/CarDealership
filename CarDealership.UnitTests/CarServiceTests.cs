using CarDealership.Core.Contracts;
using CarDealership.Infrastructure.Data;
using CarDealership.Infrastructure.Data.Common;

namespace CarDealership.UnitTests
{
    [TestFixture]
    public class CarServiceTests
    {
        private IRepository repo;
        private ICarService carService;
        private ApplicationDbContext applicationDbContext;


        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
