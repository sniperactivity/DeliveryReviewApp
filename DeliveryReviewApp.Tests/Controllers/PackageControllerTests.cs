using AutoMapper;
using DeliveryReviewApp.Contollers;
using DeliveryReviewApp.Dto;
using DeliveryReviewApp.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryReviewApp.Tests.Controllers
{
    public class PackageControllerTests
    {
        private readonly IPackage _package;
        private readonly IReview _review;
        private readonly IMapper _mapper;

        public PackageControllerTests()
        {
            _package = A.Fake<IPackage>();
            _review = A.Fake<IReview>();
            _mapper = A.Fake<IMapper>();
        }
        [Fact]
        public void PackageController_GetPackage_ReturnOk()
        {
            //Arrange
            var packages = A.Fake<ICollection<PackageDto>>();
            var packageList = A.Fake<List<PackageDto>>();
            A.CallTo(() => _mapper.Map<List<PackageDto>>(packages)).Returns(packageList);
            var controller = new PackageController(_package,_mapper,_review);

            //Act
            var result = controller.GetPackages();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }
        [Fact]
        public void PackageController_CreatePackage_ReturnActionOk()
        {
            //Arrange
            int ownerId = 1;
            int catId = 2;
            var packageMap = A.Fake<Package>();
            var package = A.Fake<Package>();
            var packageCreate = A.Fake<PackageDto>();
            var packages = A.Fake<ICollection<PackageDto>>();
            var packageList = A.Fake<IList<PackageDto>>();
            A.CallTo(() => _package.GetPackageTrimToUpper(packageCreate)).Returns(package);
            A.CallTo(() => _mapper.Map<Package>(packageCreate)).Returns(package);
            A.CallTo(() => _package.CreatePackage(ownerId, catId, packageMap)).Returns(true);
            var controller = new PackageController(_package,_mapper,_review);

            //Act
            var result = controller.CreatePackage(ownerId, catId, packageCreate);

            //Assert
            result.Should().NotBeNull();
        }
    }
}
