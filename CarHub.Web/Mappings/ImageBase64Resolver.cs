using AutoMapper;
using CarHub.Core.Entities;
using CarHub.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHub.Web.Mappings
{
    public class ImageBase64Resolver : ITypeConverter<IEnumerable<Image>, IEnumerable<string>>
    {
        public IEnumerable<string> Convert(IEnumerable<Image> source, IEnumerable<string> destination, ResolutionContext context)
        {
            List<string> base64Images = new List<string>();

            foreach (Image image in source)
            {
                base64Images.Add(Base64Utils.ConvertToBase64String(image.File));
            }

            return base64Images;
        }
    }
}
