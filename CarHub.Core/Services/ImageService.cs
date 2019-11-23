using CarHub.Core.Entities;
using CarHub.Core.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using CarHub.Core.Constants;
using System.Drawing.Imaging;

namespace CarHub.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly IAsyncRepository<Car> _carRepository;
        private readonly IAsyncRepository<Entities.Image> _imageRepository;
        private readonly IAsyncRepository<Thumbnail> _thumbnailRepository;

        public ImageService(
            IAsyncRepository<Car> carRepository, 
            IAsyncRepository<Entities.Image> imageRepository, 
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
                thumbnail.File = ResizeImage(file, ConfigurationConstants.ThumnbailWidth, ConfigurationConstants.ThumbnailHeight);
                await _thumbnailRepository.UpdateAsync(thumbnail);
            }
            else
            {
                await _thumbnailRepository.AddAsync(new Thumbnail { CarFk = carId, File = file });
            }
        }

        public byte[] ResizeImage(byte[] image, int width, int height)
        {
            Bitmap startBitmap = ByteArrayToBitmap(image);
            Bitmap newBitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(newBitmap))
            {
                graphics.DrawImage(startBitmap, 0, 0, width, height);
            }

            return BitmapToByteArray(newBitmap);
        }

        private Bitmap ByteArrayToBitmap(byte[] imageArray)
        {
            Bitmap bitmap;

            using (MemoryStream memoryStream = new MemoryStream(imageArray))
            {
                bitmap = new Bitmap(memoryStream);
            }

            return bitmap;
        }

        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Jpeg);
                return memoryStream.ToArray();
            }
        }
    }
}
