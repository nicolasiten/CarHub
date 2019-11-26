using CarHub.Core.Entities;
using CarHub.Core.Entities.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHub.Infrastructure.Tests.Repositories
{
    public class ImageRepositoryTests : FileDataRepositoryTestsBase<Image>
    {
        public ImageRepositoryTests() : base(
            new FileDataValidator<Image>(),
            new Image { File = Convert.FromBase64String("Test"), ImageType = "ImageType1" },
            new Image { File = Convert.FromBase64String("Test"), ImageType = "ImageType1" })
        { }
    }
}
