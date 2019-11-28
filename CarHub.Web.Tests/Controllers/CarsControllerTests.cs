using AutoMapper;
using CarHub.Core.Entities;
using CarHub.Core.Entities.Validations;
using CarHub.Core.Interfaces;
using CarHub.Core.Resolvers;
using CarHub.Core.Services;
using CarHub.Infrastructure.Data;
using CarHub.Infrastructure.Tests;
using CarHub.Tests.Common.Seeders;
using CarHub.Web.Controllers;
using CarHub.Web.Mappings;
using CarHub.Web.Models;
using CarHub.Web.Services;
using CarHub.Web.Utils.Alerts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarHub.Web.Tests.Controllers
{
    public class CarsControllerTests : DbContextTestBase
    {
        private readonly CarsController _carsController;
        private readonly CarModelService _carModelService;
        private readonly IAsyncRepository<Car> _carRepository;
        private readonly IAsyncRepository<Image> _imageRepository;
        private readonly IAsyncRepository<Thumbnail> _thumbnailRepository;
        private readonly IAsyncRepository<Repair> _repairRepository;

        private readonly CarModel _carModel;

        public CarsControllerTests()
        {
            Mapper mapper = new Mapper(
                new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())));
            IImageService imageService = new ImageService(
                _imageRepository,
                _thumbnailRepository,
                new ImageFormatResolver());

            _carRepository = new EfRepository<Car>(applicationDbContext, new CarValidator());
            _imageRepository = new EfRepository<Image>(applicationDbContext, new FileDataValidator<Image>());
            _thumbnailRepository = new EfRepository<Thumbnail>(applicationDbContext, new FileDataValidator<Thumbnail>());
            _repairRepository = new EfRepository<Repair>(applicationDbContext, new RepairValidator());

            _carModelService = _carModelService = new CarModelService(
                _carRepository,
                mapper,
                imageService);

            _carsController = new CarsController(
                _carModelService,
                _imageRepository,
                _thumbnailRepository,
                _repairRepository,
                imageService,
                new CarService(_carRepository));       
            
            _carModel = new CarModel
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

        }

        [Fact]
        public async Task IndexTest()
        {
            var cars = CarDataSeeder.GetEntities().ToArray();
            await _carRepository.AddAsync(cars[0]);
            await _carRepository.AddAsync(cars[1]);
            var result = await _carsController.Index();

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CarOverviewModel>(viewResult.Model);
            Assert.Equal(2, model.CarsForSale.Count());
        }

        [Fact]
        public async Task CarDetailsTest()
        {
            var car = CarDataSeeder.GetEntities().First();
            await _carRepository.AddAsync(car);
            var result = await _carsController.CarDetails(1);

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CarModel>(viewResult.Model);
            Assert.NotNull(model);
        }

        [Fact]
        public async Task CarDetailsNonExistingTest()
        {
            var result = await _carsController.CarDetails(22);

            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddCarTest()
        {
            var result = await _carsController.AddCar(_carModel);

            var alertDecoratorResult = Assert.IsType<AlertDecoratorResult>(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(alertDecoratorResult.Result);    
            Assert.Equal("Overview", redirectResult.ActionName);
            Assert.Single(await _carRepository.GetAllAsync());
        }

        [Fact]
        public async Task AddCarTestSellingPriceTooSmall()
        {
            _carModel.SellingPrice = 50;
            var result = await _carsController.AddCar(_carModel);

            var redirectResult = Assert.IsType<ViewResult>(result);
            Assert.True(_carsController.ModelState.ErrorCount == 1);
        }
    }
}
