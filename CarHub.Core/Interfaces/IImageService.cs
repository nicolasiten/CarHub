using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHub.Core.Interfaces
{
    public interface IImageService
    {
        Task SetNewThumbnailAsync(int imageId, int carId);

        byte[] ResizeImage(byte[] image, int width, int height);
    }
}
