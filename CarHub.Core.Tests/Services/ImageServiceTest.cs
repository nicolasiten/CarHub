using CarHub.Core.Constants;
using CarHub.Core.Entities;
using CarHub.Core.Entities.Validations;
using CarHub.Core.Interfaces;
using CarHub.Core.Resolvers;
using CarHub.Core.Services;
using CarHub.Infrastructure.Data;
using CarHub.Infrastructure.Tests;
using CarHub.Tests.Common.Seeders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarHub.Core.Tests.Services
{
    public class ImageServiceTests : DbContextTestBase
    {
        private readonly IImageService _imageService;
        private readonly IAsyncRepository<Entities.Image> _imageRepository;
        private readonly IAsyncRepository<Thumbnail> _thumbnailRepository;

        private readonly string _base64White;
        private readonly string _base64Black;

        public ImageServiceTests()
        {
            _imageRepository = new EfRepository<Entities.Image>(applicationDbContext, new FileDataValidator<Entities.Image>());
            _thumbnailRepository = new EfRepository<Thumbnail>(applicationDbContext, new FileDataValidator<Thumbnail>());
            _imageService = new ImageService(_imageRepository, _thumbnailRepository, new ImageFormatResolver());

            var base64Images = FileDataSeeder.GetBase64Images().ToArray();
            _base64White = base64Images[0];
            _base64Black = base64Images[1];
        }

        [Fact]
        public async Task SetNewThumbnailOverwriteExistingTest()
        {
            await _thumbnailRepository.AddAsync(new Thumbnail
            {
                CarFk = 1,
                File = Convert.FromBase64String(_base64White),
                ImageType = "png"
            });

            await _imageRepository.AddAsync(new Entities.Image
            {
                File = Convert.FromBase64String(_base64Black),
                ImageType = "png"
            });

            await _imageService.SetNewThumbnailAsync(1, 1);

            Assert.NotEqual(Convert.FromBase64String(_base64White), (await _thumbnailRepository.GetAllAsync()).Last().File);
        }

        [Fact]
        public async Task SetNewThumbnailCreateNewTest()
        {
            await _imageRepository.AddAsync(new Entities.Image
            {
                File = Convert.FromBase64String(_base64Black),
                ImageType = "png"
            });

            await _imageService.SetNewThumbnailAsync(1, 1);

            Assert.Single(await _thumbnailRepository.GetAllAsync());
        }

        [Fact]
        public void IsImageTest()
        {
            Assert.False(_imageService.IsImage(Convert.FromBase64String("TEST")));
            Assert.True(_imageService.IsImage(Convert.FromBase64String(_base64Black)));
        }

        [Fact]
        public void ResizeImageTest()
        {
            byte[] startImage = Convert.FromBase64String(_base64Black);

            byte[] resizedImage = _imageService.ResizeImage(startImage, ConfigurationConstants.ThumnbailWidth, ConfigurationConstants.ThumbnailHeight, "png");

            using (MemoryStream memoryStream = new MemoryStream(resizedImage))
            {
                Bitmap bitmap = new Bitmap(memoryStream);
                Assert.Equal(ConfigurationConstants.ThumnbailWidth, bitmap.Width);
                Assert.Equal(ConfigurationConstants.ThumbnailHeight, bitmap.Height);
            }
        }
    }
}
