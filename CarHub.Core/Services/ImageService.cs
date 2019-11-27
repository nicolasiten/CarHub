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
        private readonly IAsyncRepository<Entities.Image> _imageRepository;
        private readonly IAsyncRepository<Thumbnail> _thumbnailRepository;
        private readonly IImageFormatResolver _imageFormatResolver;

        public ImageService(
            IAsyncRepository<Entities.Image> imageRepository, 
            IAsyncRepository<Thumbnail> thumbnailRepository,
            IImageFormatResolver imageFormatResolver)
        {
            _imageRepository = imageRepository;
            _thumbnailRepository = thumbnailRepository;
            _imageFormatResolver = imageFormatResolver;
        }

        public async Task SetNewThumbnailAsync(int imageId, int carId)
        {
            var thumbnail = (await _thumbnailRepository.GetAllAsync(t => t.CarFk == carId)).SingleOrDefault();
            Entities.Image image = await _imageRepository.GetByIdAsync(imageId);
            byte[] file = ResizeImage(image.File, ConfigurationConstants.ThumnbailWidth, ConfigurationConstants.ThumbnailHeight, image.ImageType);

            if (thumbnail != null)
            {
                thumbnail.File = file;
                thumbnail.ImageType = image.ImageType;
                await _thumbnailRepository.UpdateAsync(thumbnail);
            }
            else
            {
                await _thumbnailRepository.AddAsync(new Thumbnail
                {
                    CarFk = carId,
                    File = file,
                    ImageType = image.ImageType
                });
            }
        }

        public bool IsImage(byte[] imageArray)
        {
            using (MemoryStream memoryStream = new MemoryStream(imageArray))
            {
                try
                {
                    _ = Bitmap.FromStream(memoryStream);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public byte[] ResizeImage(byte[] image, int width, int height, string imageType)
        {
            Bitmap startBitmap = ByteArrayToBitmap(image);
            Bitmap newBitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(newBitmap))
            {
                graphics.DrawImage(startBitmap, 0, 0, width, height);
            }

            return BitmapToByteArray(newBitmap, imageType);
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

        private byte[] BitmapToByteArray(Bitmap bitmap, string imageType)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, _imageFormatResolver.Resolve(imageType));
                return memoryStream.ToArray();
            }
        }
    }
}
