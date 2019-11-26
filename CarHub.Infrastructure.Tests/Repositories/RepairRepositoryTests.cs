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
    public class RepairRepositoryTests : DbContextTestBase
    {
        private readonly IAsyncRepository<Repair> _repairRepository;
        private readonly Repair _repair1;
        private readonly Repair _repair2;

        public RepairRepositoryTests()
        {
            _repairRepository = new EfRepository<Repair>(applicationDbContext, new RepairValidator());

            _repair1 = new Repair
            {
                RepairDescription = "Description1",
                RepairCost = 100
            };

            _repair2 = new Repair
            {
                RepairDescription = "Description2",
                RepairCost = 200
            };
        }

        [Fact]
        public async Task SaveRepairTest()
        {
            await _repairRepository.AddAsync(_repair1);

            var savedRepair = (await _repairRepository.GetAllAsync()).Last();
            Assert.Equal(_repair1.RepairDescription, savedRepair.RepairDescription);
            Assert.Equal(_repair1.RepairCost, savedRepair.RepairCost);
        }

        [Fact]
        public async Task GetAllRepairsTest()
        {
            await _repairRepository.AddAsync(_repair1);
            await _repairRepository.AddAsync(_repair2);

            Assert.Equal(2, (await _repairRepository.GetAllAsync()).Count());
        }

        [Fact]
        public async Task GetAllRepairsEmptyTest()
        {
            Assert.Empty(await _repairRepository.GetAllAsync());
        }

        [Fact]
        public async Task RemoveRepairTest()
        {
            await _repairRepository.AddAsync(_repair1);

            await _repairRepository.DeleteAsync(1);

            Assert.Empty(await _repairRepository.GetAllAsync());
        }

        [Fact]
        public async Task RemoveNonExistingRepairTest()
        {
            await _repairRepository.AddAsync(_repair1);

            await _repairRepository.DeleteAsync(299);

            Assert.Single(await _repairRepository.GetAllAsync());
        }

        [Fact]
        public async Task UpdateCarTest()
        {
            await _repairRepository.AddAsync(_repair1);

            var savedRepair = (await _repairRepository.GetAllAsync()).Last();
            savedRepair.RepairCost = 20;

            await _repairRepository.UpdateAsync(savedRepair);

            Assert.Equal(20, (await _repairRepository.GetAllAsync()).Last().RepairCost);
        }

        [Fact]
        public async Task ThrowsRepairDescriptionEmptyValidationException()
        {
            _repair2.RepairDescription = string.Empty;

            await Assert.ThrowsAsync<ValidationException>(async () => await _repairRepository.AddAsync(_repair2));
        }

        [Fact]
        public async Task ThrowsRepairCostValidationException()
        {
            _repair2.RepairCost = -1;

            await Assert.ThrowsAsync<ValidationException>(async () => await _repairRepository.AddAsync(_repair2));
        }
    }
}
