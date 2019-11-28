using AutoMapper;
using CarHub.Core.Constants;
using CarHub.Core.Entities;
using CarHub.Core.Entities.Validations;
using CarHub.Core.Resolvers;
using CarHub.Core.Services;
using CarHub.Infrastructure.Data;
using CarHub.Infrastructure.Tests;
using CarHub.Tests.Common.Seeders;
using CarHub.Web.Interfaces;
using CarHub.Web.Mappings;
using CarHub.Web.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CarHub.Web.Tests.Services
{
    public class CarModelServiceTests : DbContextTestBase
    {
        private readonly ICarModelService _carModelService;

        private readonly dynamic _jObject1;
        private readonly dynamic _jObject2;

        public CarModelServiceTests()
        {
            _carModelService = new CarModelService(
                new EfRepository<Car>(applicationDbContext, new CarValidator()),
                new Mapper(
                    new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()))),
                    new ImageService(
                        new EfRepository<Image>(applicationDbContext, new FileDataValidator<Image>()),
                        new EfRepository<Thumbnail>(applicationDbContext, new FileDataValidator<Thumbnail>()),
                        new ImageFormatResolver()));

            var images = FileDataSeeder.GetBase64Images().ToArray();

            _jObject1 = new JObject();
            _jObject1.data = new JObject(new JProperty("Value", images[0]));
            _jObject1.type = "png";
            _jObject1.size = new JObject(new JProperty("Value", 1000000));

            _jObject2 = new JObject();
            _jObject2.data = new JObject(new JProperty("Value", images[1]));
            _jObject2.type = "png";
            _jObject2.size = new JObject(new JProperty("Value", 2000000));
        }

        [Fact]
        public void ValidateCarImagesValidTest()
        {
            Assert.Empty(_carModelService.ValidateCarImages(new string[] { _jObject1.ToString(), _jObject2.ToString() }));
        }

        [Fact]
        public void ValidateCarImagesFilesMustBeImagesTest()
        {
            _jObject1.data = new JObject(new JProperty("Value", "Invalid image"));

            var errors = _carModelService.ValidateCarImages(new string[] { _jObject1.ToString() });
            Assert.Equal("Uploaded files must be images!", errors.Single());
        }

        [Fact]
        public void ValidateCarImagesFilesMustBeOfTypeTest()
        {
            _jObject1.type = "docx";

            var errors = _carModelService.ValidateCarImages(new string[] { _jObject1.ToString() });
            Assert.Equal($"Images must be of type {string.Join(" or ", ConfigurationConstants.SupportedImageFormats)}.", errors.Single());
        }

        [Fact]
        public void ValidateCarImagesFilesMustBeSmallerThanTest()
        {
            _jObject1.size = new JObject(new JProperty("Value", 5000001));

            var errors = _carModelService.ValidateCarImages(new string[] { _jObject1.ToString() });
            Assert.Equal("Images must not be bigger than 5MB.", errors.Single());
        }
    }
}
