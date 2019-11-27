using CarHub.Core.Interfaces;
using CarHub.Core.Resolvers;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;
using Xunit;

namespace CarHub.Core.Tests.Resolvers
{
    public class ImageFormatResolverTests
    {
        private readonly IImageFormatResolver _imageFormatResolver;

        private static readonly ImageFormat pngImageFormat = ImageFormat.Png;

        public ImageFormatResolverTests()
        {
            _imageFormatResolver = new ImageFormatResolver();
        }

        [Fact]
        public void ResolvePngFormatTest()
        {
            Assert.Equal(ImageFormat.Png, _imageFormatResolver.Resolve("png"));
            Assert.Equal(ImageFormat.Png, _imageFormatResolver.Resolve("PNG"));
        }

        [Fact]
        public void ResolveJpgFormatTest()
        {
            Assert.Equal(ImageFormat.Jpeg, _imageFormatResolver.Resolve("jpg"));
            Assert.Equal(ImageFormat.Jpeg, _imageFormatResolver.Resolve("jpeg"));
        }

        [Fact]
        public void ResolveBmpFormatTest()
        {
            Assert.Equal(ImageFormat.Bmp, _imageFormatResolver.Resolve("bmp"));
        }

        [Fact]
        public void ResolveThrowsTest()
        {
            Assert.Throws<NotSupportedException>(() => _imageFormatResolver.Resolve(".docx"));
        }
    }
}
