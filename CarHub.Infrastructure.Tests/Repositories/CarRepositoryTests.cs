using CarHub.Core.Entities;
using CarHub.Core.Entities.Validations;
using CarHub.Core.Interfaces;
using CarHub.Infrastructure.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarHub.Infrastructure.Tests.Repositories
{
    public class CarRepositoryTests : DbContextTestBase
    {
        private readonly IAsyncRepository<Car> _carRepository;
        private readonly Car _car1;
        private readonly Car _car2;

        public CarRepositoryTests()
        {
            _carRepository = new EfRepository<Car>(applicationDbContext, new CarValidator());

            _car1 = new Car
            {
                Description = "Description",
                Kilometers = 100,
                LotDate = new DateTime(2019, 12, 1),
                PurchaseDate = new DateTime(2019, 11, 30),
                Make = "Make",
                Model = "Model",
                Trim = "Trim",
                TransmissionType = Core.Enums.TransmissionType.Automatic,
                Vin = "11111111111111111",
                PurchasePrice = 1000,
                SellingPrice = 2000,
                ShowCase = true,
                Year = 2002,
                ThumbnailImage = new Thumbnail
                {
                    ImageType = "ImageType",
                    File = Convert.FromBase64String("TEST")
                },
                Repairs = new List<Repair>
                {
                    new Repair
                    {
                        RepairDescription = "Description",
                        RepairCost = 100
                    }
                },
                Images = new List<Image>
                {
                    new Image
                    {
                        ImageType = "ImageType",
                        File = Convert.FromBase64String("TEST")
                    }
                }
            };

            _car2 = new Car
            {
                Description = "Description1",
                Kilometers = 200,
                LotDate = new DateTime(2019, 11, 12),
                PurchaseDate = new DateTime(2019, 11, 10),
                Make = "Make1",
                Model = "Model1",
                Trim = "Trim1",
                TransmissionType = Core.Enums.TransmissionType.Manual,
                Vin = "22222222222222222",
                PurchasePrice = 5000,
                SellingPrice = 7000,
                ShowCase = false,
                Year = 2012,
            };
        }

        [Fact]
        public async Task SaveCarCompleteTest()
        {
            await _carRepository.AddAsync(_car1);

            var savedCar = (await _carRepository.GetAllAsync()).Last();
            Assert.Equal(_car1.Description, savedCar.Description);
            Assert.Equal(_car1.Kilometers, savedCar.Kilometers);
            Assert.Equal(_car1.LotDate, savedCar.LotDate);
            Assert.Equal(_car1.PurchaseDate, savedCar.PurchaseDate);
            Assert.Equal(_car1.Make, savedCar.Make);
            Assert.Equal(_car1.Model, savedCar.Model);
            Assert.Equal(_car1.Trim, savedCar.Trim);
            Assert.Equal(_car1.TransmissionType, savedCar.TransmissionType);
            Assert.Equal(_car1.Vin, savedCar.Vin);
            Assert.Equal(_car1.PurchaseDate, savedCar.PurchaseDate);
            Assert.Equal(_car1.SellingPrice, savedCar.SellingPrice);
            Assert.Equal(_car1.ShowCase, savedCar.ShowCase);
            Assert.Equal(_car1.Year, savedCar.Year);
            Assert.Equal(_car1.ThumbnailImage.File, savedCar.ThumbnailImage.File);
            Assert.Equal(_car1.ThumbnailImage.ImageType, savedCar.ThumbnailImage.ImageType);
            Assert.Equal(_car1.Repairs.First().RepairDescription, savedCar.Repairs.First().RepairDescription);
            Assert.Equal(_car1.Repairs.First().RepairCost, savedCar.Repairs.First().RepairCost);
            Assert.Equal(_car1.Images.First().File, savedCar.Images.First().File);
            Assert.Equal(_car1.Images.First().ImageType, savedCar.Images.First().ImageType);
        }

        [Fact]
        public async Task SaveCarWithoutRepairsImagesTest()
        {
            await _carRepository.AddAsync(_car2);

            var savedCar = (await _carRepository.GetAllAsync()).Last();
            Assert.Equal(_car2.Description, savedCar.Description);
            Assert.Equal(_car2.Kilometers, savedCar.Kilometers);
            Assert.Equal(_car2.LotDate, savedCar.LotDate);
            Assert.Equal(_car2.PurchaseDate, savedCar.PurchaseDate);
            Assert.Equal(_car2.Make, savedCar.Make);
            Assert.Equal(_car2.Model, savedCar.Model);
            Assert.Equal(_car2.Trim, savedCar.Trim);
            Assert.Equal(_car2.TransmissionType, savedCar.TransmissionType);
            Assert.Equal(_car2.Vin, savedCar.Vin);
            Assert.Equal(_car2.PurchaseDate, savedCar.PurchaseDate);
            Assert.Equal(_car2.SellingPrice, savedCar.SellingPrice);
            Assert.Equal(_car2.ShowCase, savedCar.ShowCase);
            Assert.Equal(_car2.Year, savedCar.Year);
            Assert.Null(savedCar.ThumbnailImage);
            Assert.Empty(savedCar.Repairs);
            Assert.Empty(savedCar.Images);
        }

        [Fact]
        public async Task GetAllCarsTest()
        {
            await _carRepository.AddAsync(_car1);
            await _carRepository.AddAsync(_car2);

            Assert.Equal(2, (await _carRepository.GetAllAsync()).Count());
        }

        [Fact]
        public async Task GetAllCarsEmptyTest()
        {
            Assert.Empty(await _carRepository.GetAllAsync());
        }

        [Fact]
        public async Task RemoveCarTest()
        {
            await _carRepository.AddAsync(_car2);

            await _carRepository.DeleteAsync(1);

            Assert.Empty(await _carRepository.GetAllAsync());
        }

        [Fact]
        public async Task RemoveNonExistingCarTest()
        {
            await _carRepository.AddAsync(_car1);

            await _carRepository.DeleteAsync(199);

            Assert.Single(await _carRepository.GetAllAsync());
        }

        [Fact]
        public async Task UpdateCarTest()
        {
            await _carRepository.AddAsync(_car1);

            var savedCar = (await _carRepository.GetAllAsync()).Last();
            savedCar.Year = 2019;

            await _carRepository.UpdateAsync(savedCar);

            Assert.Equal(2019, (await _carRepository.GetAllAsync()).Last().Year);
        }

        [Fact]
        public async Task ThrowsVinValidationException()
        {
            _car2.Vin = "222";

            await Assert.ThrowsAsync<ValidationException>(async () => await _carRepository.AddAsync(_car2));
        }

        [Fact]
        public async Task ThrowsYearValidationException()
        {
            _car2.Year = 1900;

            await Assert.ThrowsAsync<ValidationException>(async () => await _carRepository.AddAsync(_car2));
        }

        [Fact]
        public async Task ThrowsMakeEmptyValidationException()
        {
            _car2.Make = string.Empty;

            await Assert.ThrowsAsync<ValidationException>(async () => await _carRepository.AddAsync(_car2));
        }

        [Fact]
        public async Task ThrowsModelEmptyValidationException()
        {
            _car2.Model = string.Empty;

            await Assert.ThrowsAsync<ValidationException>(async () => await _carRepository.AddAsync(_car2));
        }

        [Fact]
        public async Task ThrowsTrimEmptyValidationException()
        {
            _car2.Trim = string.Empty;

            await Assert.ThrowsAsync<ValidationException>(async () => await _carRepository.AddAsync(_car2));
        }

        [Fact]
        public async Task ThrowsDescriptionEmptyValidationException()
        {
            _car2.Description = string.Empty;

            await Assert.ThrowsAsync<ValidationException>(async () => await _carRepository.AddAsync(_car2));
        }

        [Fact]
        public async Task ThrowsKilometersValidationException()
        {
            _car2.Kilometers = -1;

            await Assert.ThrowsAsync<ValidationException>(async () => await _carRepository.AddAsync(_car2));
        }

        [Fact]
        public async Task ThrowsSellingPriceValidationException()
        {
            _car2.SellingPrice = -1;

            await Assert.ThrowsAsync<ValidationException>(async () => await _carRepository.AddAsync(_car2));
        }

        [Fact]
        public async Task ThrowsLotDatePurchaseDateValidationException()
        {
            _car2.PurchaseDate = new DateTime(2019, 12, 15);
            _car2.LotDate = new DateTime(2019, 12, 14);

            await Assert.ThrowsAsync<ValidationException>(async () => await _carRepository.AddAsync(_car2));
        }
    }
}
