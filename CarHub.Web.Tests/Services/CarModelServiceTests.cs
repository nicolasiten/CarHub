using AutoMapper;
using CarHub.Core.Constants;
using CarHub.Core.Entities;
using CarHub.Core.Entities.Validations;
using CarHub.Core.Interfaces;
using CarHub.Core.Resolvers;
using CarHub.Core.Services;
using CarHub.Infrastructure.Data;
using CarHub.Infrastructure.Tests;
using CarHub.Tests.Common.Seeders;
using CarHub.Web.Interfaces;
using CarHub.Web.Mappings;
using CarHub.Web.Models;
using CarHub.Web.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarHub.Web.Tests.Services
{
    public class CarModelServiceTests : DbContextTestBase
    {
        private readonly ICarModelService _carModelService;
        private readonly IAsyncRepository<Car> _carRepository;

        private readonly dynamic _jObject1;
        private readonly dynamic _jObject2;

        public CarModelServiceTests()
        {
            _carRepository = new EfRepository<Car>(applicationDbContext, new CarValidator());

            _carModelService = new CarModelService(
                _carRepository,
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

        [Fact]
        public async Task SaveCarModelWithImagesTest()
        {
            var carModel = new CarModel
            {
                Description = "Description",
                Kilometers = 1000,
                PurchaseDate = new DateTime(2019, 12, 10),
                PurchasePrice = 2000,
                LotDate = new DateTime(2019, 12, 12),
                Make = "Make",
                Model = "Model",
                Trim = "Trim",
                SellingPrice = 4000,
                Vin = "11111111111111111",
                ShowCase = false,
                Year = 2005,
                TransmissionType = Core.Enums.TransmissionType.Automatic
            };

            await _carModelService.SaveCarModelAsync(carModel, new string[] { _jObject1.ToString() });

            var savedCar = (await _carRepository.GetAllAsync(includeProperties: "ThumbnailImage,Images")).Last();

            Assert.Equal(carModel.Description, savedCar.Description);
            Assert.Equal(carModel.Kilometers, savedCar.Kilometers);
            Assert.Equal(carModel.PurchaseDate, savedCar.PurchaseDate);
            Assert.Equal(carModel.PurchasePrice, savedCar.PurchasePrice);
            Assert.Equal(carModel.LotDate, savedCar.LotDate);
            Assert.Equal(carModel.Make, savedCar.Make);
            Assert.Equal(carModel.Model, savedCar.Model);
            Assert.Equal(carModel.Trim, savedCar.Trim);
            Assert.Equal(carModel.SellingPrice, savedCar.SellingPrice);
            Assert.Equal(carModel.Vin, savedCar.Vin);
            Assert.Equal(carModel.ShowCase, savedCar.ShowCase);
            Assert.Equal(carModel.Year, savedCar.Year);
            Assert.Equal(carModel.TransmissionType, savedCar.TransmissionType);
            Assert.NotNull(savedCar.ThumbnailImage);
            Assert.Single(savedCar.Images);
        }

        [Fact]
        public async Task GetCarModelsTest()
        {
            var cars = CarDataSeeder.GetEntities().ToArray();
            await _carRepository.AddAsync(cars[0]);
            await _carRepository.AddAsync(cars[1]);

            Assert.Equal(2, (await _carModelService.GetCarModelsAsync()).Count());
        }

        [Fact]
        public async Task GetCarModelsEmptyTest()
        {
            Assert.Empty(await _carModelService.GetCarModelsAsync());
        }

        [Fact]
        public async Task GetCarModelByIdTest()
        {
            var car = CarDataSeeder.GetEntities().First();
            await _carRepository.AddAsync(car);

            Assert.NotNull(await _carModelService.GetCarModelByIdAsync(1));
        }

        [Fact]
        public async Task GetCarModelByIdNonExistingTest()
        {
            Assert.Null(await _carModelService.GetCarModelByIdAsync(22));
        }

        [Fact]
        public async Task GetCarOverviewModelTest()
        {
            var car = CarDataSeeder.GetEntities().First();
            car.ShowCase = false;
            await _carRepository.AddAsync(car);
            car.Id = 0;
            car.ShowCase = true;
            car.SaleDate = DateTime.Today.AddDays(-(ConfigurationConstants.RecentlySoldMaxDays + 1));
            await _carRepository.AddAsync(car);
            car.Id = 0;
            car.ShowCase = false;
            car.SaleDate = DateTime.Today;
            await _carRepository.AddAsync(car);

            var carOverviewModel = await _carModelService.GetCarOverviewModelAsync();
            Assert.Single(carOverviewModel.CarsForSale);
            Assert.Single(carOverviewModel.CarsShowcase);
            Assert.Single(carOverviewModel.CarsRecentlySold);
        }

        [Fact]
        public async Task GetCarModelsAdminOverviewTest()
        {
            var cars = CarDataSeeder.GetEntities().ToArray();
            await _carRepository.AddAsync(cars[0]);
            await _carRepository.AddAsync(cars[1]);

            Assert.Equal(2, cars.Count());
        }

        [Fact]
        public async Task GetCarModelsAdminOverviewEmptyTest()
        {
            Assert.Empty(await _carModelService.GetCarModelsAdminOverview());
        }

        [Fact]
        public async Task CalculateTotalPurchasePriceTest()
        {
            var car = CarDataSeeder.GetEntities().First();
            await _carRepository.AddAsync(car);

            var carModel = (await _carModelService.GetCarModelsAsync()).Last();

            Assert.Equal(1100, _carModelService.CalculateTotalPurchasePrice(carModel));
        }

        [Fact]
        public async Task CalculateTotalPurchasePriceNoRepairsTest()
        {
            var car = CarDataSeeder.GetEntities().First();
            car.Repairs = new Repair[] { };
            await _carRepository.AddAsync(car);

            var carModel = (await _carModelService.GetCarModelsAsync()).Last();

            Assert.Equal(1000, _carModelService.CalculateTotalPurchasePrice(carModel));
        }
    }
}
