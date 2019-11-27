using CarHub.Core.Constants;
using CarHub.Core.Entities;
using CarHub.Core.Entities.Validations;
using CarHub.Core.Interfaces;
using CarHub.Core.Resolvers;
using CarHub.Core.Services;
using CarHub.Infrastructure.Data;
using CarHub.Infrastructure.Tests;
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
    public class ImageServiceTest : DbContextTestBase
    {
        private readonly IImageService _imageService;
        private readonly IAsyncRepository<Entities.Image> _imageRepository;
        private readonly IAsyncRepository<Thumbnail> _thumbnailRepository;

        private readonly string _base64White;
        private readonly string _base64Black;

        public ImageServiceTest()
        {
            _imageRepository = new EfRepository<Entities.Image>(applicationDbContext, new FileDataValidator<Entities.Image>());
            _thumbnailRepository = new EfRepository<Thumbnail>(applicationDbContext, new FileDataValidator<Thumbnail>());
            _imageService = new ImageService(_imageRepository, _thumbnailRepository, new ImageFormatResolver());

            _base64White = "iVBORw0KGgoAAAANSUhEUgAAAfQAAAFNCAIAAAB5cQpgAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAR7SURBVHhe7dQBDQAADMOg+ze9+2hABDcAcuQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBI7gBBcgcIkjtAkNwBguQOECR3gCC5AwTJHSBne8vk/q40rO5qAAAAAElFTkSuQmCC";
            _base64Black = "iVBORw0KGgoAAAANSUhEUgAAAfQAAAFNCAIAAAB5cQpgAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAH7SURBVHhe7cExAQAAAMKg9U9tCj8gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB5qoOIAAQFnx2kAAAAASUVORK5CYII=";
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
