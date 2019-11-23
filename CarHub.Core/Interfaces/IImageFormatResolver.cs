using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace CarHub.Core.Interfaces
{
    public interface IImageFormatResolver
    {
        ImageFormat Resolve(string imageFormat);
    }
}
