using DeliveryReviewApp.Data;
using DeliveryReviewApp.Models;
using DeliveryReviewApp.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryReviewApp.Tests.Repository
{
    public class PackageRepoTests
    {
        private async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Packages.CountAsync() <= 0)
            {
                for (int i = 1; i <= 10; i++)
                {
                    databaseContext.Packages.Add(
                    new Package()
                    {
                        Name = "Phone",
                        DropDate = new DateTime(1903, 1, 1),
                        PackageCategories = new List<PackageCategory>()
                            {
                                new PackageCategory { Category = new Category() { Name = "Electric"}}
                            },
                        Reviews = new List<Review>()
                            {
                                new Review { Title="Fastest Delivery",Text = "DoyleDelivery is the best delivery company ever, because it is Fast", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Sniper", LastName = "Yak" } },
                                new Review { Title="Respectful", Text = "driver was nice and respectful", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title="Easy",Text = "Easiest delivery proccess ever!!!", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "James", LastName = "Ade" } },
                            }
                    });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }
        [Fact]
        public async void PackageRepo_GetPackage_ReturnsPackage()
        {
            //Arrange
            var name = "Phone";
            var dbContext = await GetDatabaseContext();
            var packageRepo = new PackageRepo(dbContext);

            //Act
            var result = packageRepo.GetPackage(name);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Package>();
        }
        [Fact]
        public async void PackageRepo_GetPackageRating_ReturnsDecimalBetweenOneAndTen()
        {
            //Arrange
            var packId = 1;
            var dbContext = await GetDatabaseContext();
            var packageRepo = new PackageRepo(dbContext);

            //Act
            var result = packageRepo.GetDeliveryRating(packId);

            //Assert
            result.Should().NotBe(0);
            result.Should().BeInRange(1, 10);
        }
    }
}
