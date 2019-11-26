using CarHub.Core.Entities;
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
    public abstract class FileDataRepositoryTestsBase<T> : DbContextTestBase where T : FileData, new()
    {
        protected readonly IAsyncRepository<T> repository;
        protected readonly T item1;
        protected readonly T item2;

        protected FileDataRepositoryTestsBase(IValidator<T> validator, T item1, T item2)
        {
            repository = new EfRepository<T>(applicationDbContext, validator);
            this.item1 = item1;
            this.item2 = item2;
        }

        [Fact]
        public async Task SaveFileDataTest()
        {
            await repository.AddAsync(item1);

            var savedItem = (await repository.GetAllAsync()).Last();
            Assert.Equal(item1.File, savedItem.File);
            Assert.Equal(item1.ImageType, savedItem.ImageType);
        }

        [Fact]
        public async Task GetAllFileDataTest()
        {
            await repository.AddAsync(item1);
            await repository.AddAsync(item2);

            Assert.Equal(2, (await repository.GetAllAsync()).Count());
        }

        [Fact]
        public async Task GetAllFileDataEmtpyTest()
        {
            Assert.Empty(await repository.GetAllAsync());
        }

        [Fact]
        public async Task RemoveFileDataTest()
        {
            await repository.AddAsync(item1);

            await repository.DeleteAsync(1);

            Assert.Empty(await repository.GetAllAsync());
        }

        [Fact]
        public async Task RemoveNonExistingFileDataTest()
        {
            await repository.AddAsync(item1);

            await repository.DeleteAsync(233);

            Assert.Single(await repository.GetAllAsync());
        }

        [Fact]
        public async Task UpdateFileDataTest()
        {
            await repository.AddAsync(item1);

            var savedItem = ((await repository.GetAllAsync()).Last());
            savedItem.ImageType = "newType";

            await repository.UpdateAsync(savedItem);

            Assert.Equal("newType", (await repository.GetAllAsync()).Last().ImageType);
        }

        [Fact]
        public async Task ThrowsFileDataImageTypeException()
        {
            item1.ImageType = null;

            await Assert.ThrowsAsync<ValidationException>(async () => await repository.AddAsync(item1));
        }

        [Fact]
        public async Task ThrowsFileDataFileException()
        {
            item1.File = null;

            await Assert.ThrowsAsync<ValidationException>(async () => await repository.AddAsync(item1));
        }
    }
}
