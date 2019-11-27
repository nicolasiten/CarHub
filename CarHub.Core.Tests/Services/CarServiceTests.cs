using CarHub.Core.Entities;
using CarHub.Core.Entities.Validations;
using CarHub.Core.Interfaces;
using CarHub.Core.Services;
using CarHub.Infrastructure.Data;
using CarHub.Infrastructure.Tests;
using CarHub.Tests.Common.Seeders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarHub.Core.Tests.Services
{
    public class CarServiceTests : DbContextTestBase
    {
        private readonly IAsyncRepository<Car> _carRepository;
        private readonly ICarService _carService;
        private readonly Car _car;

        public CarServiceTests()
        {
            _carRepository = new EfRepository<Car>(applicationDbContext, new CarValidator());
            _carService = new CarService(_carRepository);
            _car = CarDataSeeder.GetEntities().First();
        }

        [Fact]
        public async Task SetSalesDateAsyncTest()
        {
            await _carRepository.AddAsync(_car);

            Assert.Null((await _carRepository.GetAllAsync()).Last().SaleDate);

            var saleDate = new DateTime(2019, 12, 10);
            await _carService.SetSalesDateAsync(1, saleDate);

            Assert.Equal(saleDate, (await _carRepository.GetAllAsync()).Last().SaleDate);
        }

        [Fact]
        public async Task SetSalesDateNonExistingCarTest()
        {
            await _carRepository.AddAsync(_car);

            await _carService.SetSalesDateAsync(232, new DateTime(2019, 12, 10));

            Assert.Null((await _carRepository.GetAllAsync()).Last().SaleDate);
        }
    }
}
