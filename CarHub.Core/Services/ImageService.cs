using CarHub.Core.Entities;
using CarHub.Core.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHub.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly IAsyncRepository<Car> _carRepository;
        private readonly IAsyncRepository<Image> _imageRepository;
        private readonly IAsyncRepository<Thumbnail> _thumbnailRepository;

        public ImageService(
            IAsyncRepository<Car> carRepository, 
            IAsyncRepository<Image> imageRepository, 
            IAsyncRepository<Thumbnail> thumbnailRepository)
        {
            _carRepository = carRepository;
            _imageRepository = imageRepository;
            _thumbnailRepository = thumbnailRepository;
        }

        public async Task SetNewThumbnailAsync(int imageId, int carId)
        {
            var thumbnail = (await _thumbnailRepository.GetAllAsync(t => t.CarFk == carId)).SingleOrDefault();
            byte[] file = (await _imageRepository.GetByIdAsync(imageId)).File;

            if (thumbnail != null)
            {
                thumbnail.File = file;
                await _thumbnailRepository.UpdateAsync(thumbnail);
            }
            else
            {
                await _thumbnailRepository.AddAsync(new Thumbnail { CarFk = carId, File = file });
            }
        }
    }
}
