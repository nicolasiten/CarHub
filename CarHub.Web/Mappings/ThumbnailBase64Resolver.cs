using AutoMapper;
using CarHub.Core.Entities;
using CarHub.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHub.Web.Mappings
{
    public class ThumbnailBase64Resolver : ITypeConverter<Thumbnail, string>
    {
        public string Convert(Thumbnail source, string destination, ResolutionContext context)
        {
            return Base64Utils.ConvertToBase64String(source.File);
        }
    }
}
