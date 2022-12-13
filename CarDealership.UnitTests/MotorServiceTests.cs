using CarDealership.Core.Contracts;
using CarDealership.Core.Models.Motor;
using CarDealership.Core.Services;
using CarDealership.Infrastructure.Data;
using CarDealership.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CarDealership.UnitTests
{
    [TestFixture]
    public class MotorServiceTests
    {
        private IRepository repo;
        private IMotorService motorService;
        private ApplicationDbContext context;


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase("MotorDealershipDB")
               .Options;

            context = new ApplicationDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public async Task TestAll()
        {
            var repo = new Repository(context);
            motorService = new MotorService(repo);

            await repo.AddRangeAsync(new List<Motor>()
            {
                new Motor { Id = 1, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 2, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 3, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 4, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 5, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 6, Model = "", Description = "", ImageUrl = ""},

            });

            await repo.SaveChangesAsync();

            var result = await motorService.All();

           
            Assert.That(3, Is.EqualTo(result.Motors.Count()));
        }

        [Test]
        public async Task AllCarsByDealerId()
        {
            var repo = new Repository(context);
            motorService = new MotorService(repo);

            await repo.AddRangeAsync(new List<Motor>()
            {
                new Motor { Id = 1, Model = "", Description = "", ImageUrl = "", DealerId = 1, IsActive = true},
                new Motor { Id = 2, Model = "", Description = "", ImageUrl = "", DealerId = 1, IsActive = true},
                new Motor { Id = 3, Model = "", Description = "", ImageUrl = "", DealerId = 2, IsActive = true}           
                

            });

            await repo.SaveChangesAsync();

            var result = await motorService.AllMotorsByDealerId(1);

            Assert.That(2, Is.EqualTo(result.Count()));            
        }

        [Test]
        public async Task AllCarsByUserId()
        {
            var repo = new Repository(context);
            motorService = new MotorService(repo);

            await repo.AddRangeAsync(new List<Motor>()
            {
                new Motor { Id = 1, Model = "", Description = "", ImageUrl = "", BuyerId = "1", IsActive = true},
                new Motor { Id = 2, Model = "", Description = "", ImageUrl = "", BuyerId = "1", IsActive = true},
                new Motor { Id = 3, Model = "", Description = "", ImageUrl = "", BuyerId = "2", IsActive = true}


            });

            await repo.SaveChangesAsync();

            var result = await motorService.AllMotorsByUserId("1");
            Assert.That(2, Is.EqualTo(result.Count()));

        }

        [Test]
        public async Task TestAllCategories()
        {
            var repo = new Repository(context);
            motorService = new MotorService(repo);
            await repo.AddRangeAsync(new List<MotorCategory>()
            {
                new MotorCategory { Id =1, Name = "1"}
            });

            await repo.SaveChangesAsync();

            var result = await motorService.AllCategories();
            Assert.That(1, Is.EqualTo(result.Count()));

            var resultCatNames = await motorService.AllCategoriesNames();
            Assert.That(resultCatNames, Is.Not.Null);

        }

        [Test]
        public async Task TestCategoryExists()
        {
            var repo = new Repository(context);
            motorService = new MotorService(repo);
            await repo.AddRangeAsync(new List<Motor>()
            {
                new Motor { Id = 1, Model = "", Description = "", ImageUrl = "", DealerId = 1, IsActive = true},
                new Motor { Id = 2, Model = "", Description = "", ImageUrl = "", DealerId = 1, IsActive = true},
                new Motor { Id = 3, Model = "", Description = "", ImageUrl = "", DealerId = 2, IsActive = true}


            });
            await repo.SaveChangesAsync();

            await repo.AddRangeAsync(new List<MotorCategory>()
            {
                new MotorCategory { Id =1, Name = "1"}
            });

            await repo.SaveChangesAsync();

            var catId = await motorService.GetMotorCategoryId(1);
            var result = await motorService.CategoryExists(catId);

            Assert.That(result, Is.False);
        }

       

        [Test]
        public async Task TestLastFive()
        {
            var repo = new Repository(context);
            motorService = new MotorService(repo);

            await repo.AddRangeAsync(new List<Motor>()
            {
                new Motor { Id = 1, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 2, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 3, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 4, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 5, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 6, Model = "", Description = "", ImageUrl = ""},

            });

            await repo.SaveChangesAsync();

            var result = await motorService.LastFiveMotors();
           
           Assert.That(5, Is.EqualTo(result.Count()));
           Assert.That(result.Any(c => c.Id == 1), Is.False);

        }

        [Test]
        public async Task TestExists_IsBought()
        {
            var repo = new Repository(context);
            motorService = new MotorService(repo);

            await repo.AddRangeAsync(new List<Motor>()
            {
                new Motor { Id = 1, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 2, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 3, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 4, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 5, Model = "", Description = "", ImageUrl = ""},
                new Motor { Id = 6, Model = "", Description = "", ImageUrl = ""},

            });

            await repo.SaveChangesAsync();

            var result = await motorService.Exists(1);

            Assert.That(result, Is.True);

            var result1 = await motorService.Exists(7);
            Assert.That(result1, Is.False);

            var result3 = await motorService.IsBought(2);

            Assert.That(result3, Is.False);
        }

       

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
