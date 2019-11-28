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

            _carRepository = new EfRepository<Car>(applicationDbContext, new CarValidator());
            _imageRepository = new EfRepository<Image>(applicationDbContext, new FileDataValidator<Image>());
            _thumbnailRepository = new EfRepository<Thumbnail>(applicationDbContext, new FileDataValidator<Thumbnail>());
            _repairRepository = new EfRepository<Repair>(applicationDbContext, new RepairValidator());

            IImageService imageService = new ImageService(
                _imageRepository,
                _thumbnailRepository,
                new ImageFormatResolver());

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

            Assert.IsType<ViewResult>(result);
            Assert.True(_carsController.ModelState.ErrorCount == 1);
        }

        [Fact]
        public async Task EditCarTest()
        {
            var car = CarDataSeeder.GetEntities().First();
            await _carRepository.AddAsync(car);
            var result = await _carsController.EditCar(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
        }

        [Fact]
        public async Task EditCarNonExistingTest()
        {
            var result = await _carsController.EditCar(22);

            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task EditCarSaveTest()
        {
            await _carsController.AddCar(_carModel);

            _carModel.Id = 1;
            _carModel.Description = "NewDescription";
            var saveResult = await _carsController.EditCar(_carModel);
            Assert.IsType<AlertDecoratorResult>(saveResult);
            Assert.Equal("NewDescription", (await _carRepository.GetByIdAsync(1)).Description);
        }

        [Fact]
        public async Task OverviewTest()
        {
            var cars = CarDataSeeder.GetEntities().ToArray();
            await _carRepository.AddAsync(cars[0]);
            await _carRepository.AddAsync(cars[1]);
            var result = await _carsController.Overview();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<CarModel>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task RemoveRepairTest()
        {
            var car = CarDataSeeder.GetEntities().First();
            await _carRepository.AddAsync(car);
            var result = await _carsController.RemoveRepair(1);

            Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Empty((await _carRepository.GetAllAsync()).Last().Repairs);
        }

        [Fact]
        public async Task RemoveNonExistingRepairTest()
        {
            var result = await _carsController.RemoveRepair(22);

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task RemoveImageTest()
        {
            var car = CarDataSeeder.GetEntities().First();
            await _carRepository.AddAsync(car);
            var result = await _carsController.RemoveImage(1);

            Assert.IsAssignableFrom<JsonResult>(result);
            Assert.Empty((await _carRepository.GetAllAsync()).Last().Images);
        }

        [Fact]
        public async Task RemoveImageNonExistingTest()
        {
            var result = await _carsController.RemoveImage(22);

            Assert.IsAssignableFrom<JsonResult>(result);
        }

        [Fact]
        public async Task SetAsThumbnailTest()
        {
            var car = CarDataSeeder.GetEntities().First();            
            car.Images.Add(new Image { File = Convert.FromBase64String(FileDataSeeder.GetBase64Images().Last()), ImageType = "png" });
            await _carRepository.AddAsync(car);
            var result = await _carsController.SetAsThumbnail(2, 1);

            Assert.IsAssignableFrom<JsonResult>(result);
            Assert.Equal(car.ThumbnailImage.File, (await _carRepository.GetByIdAsync(1)).ThumbnailImage.File);
        }

        [Fact]
        public async Task SetSalesDate()
        {
            var car = CarDataSeeder.GetEntities().First();
            await _carRepository.AddAsync(car);
            var result = await _carsController.SetSalesDate(1, DateTime.Today);

            Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal(DateTime.Today, (await _carRepository.GetAllAsync()).Last().SaleDate);
        }
    }
}
