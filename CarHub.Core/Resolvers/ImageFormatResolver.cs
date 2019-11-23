using CarHub.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace CarHub.Core.Resolvers
{
    public class ImageFormatResolver : IImageFormatResolver
    {
        public ImageFormat Resolve(string imageFormat)
        {
            if (imageFormat.Contains("png"))
            {
                return ImageFormat.Png;
            }
            else if (imageFormat.Contains("jpg") || imageFormat.Contains("jpeg"))
            {
                return ImageFormat.Jpeg;
            }
            else if (imageFormat.Contains("bmp"))
            {
                return ImageFormat.Bmp;
            }

            throw new NotSupportedException(imageFormat);
        }
    }
}
