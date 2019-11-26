using CarHub.Core.Entities;
using CarHub.Core.Entities.Validations;
using CarHub.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHub.Infrastructure.Tests.Repositories
{
    public class ThumbnailRepositoryTests : FileDataRepositoryTestsBase<Thumbnail>
    {
        public ThumbnailRepositoryTests() : base(
            new FileDataValidator<Thumbnail>(),
            new Thumbnail { File = Convert.FromBase64String("Test"), ImageType = "ImageType1" },
            new Thumbnail { File = Convert.FromBase64String("Test"), ImageType = "ImageType1" })
        { }
    }
}
