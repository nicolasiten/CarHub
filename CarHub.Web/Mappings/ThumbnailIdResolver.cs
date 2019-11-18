using AutoMapper;
using CarHub.Core.Entities;
using CarHub.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHub.Web.Mappings
{
    public class ThumbnailIdResolver : ITypeConverter<Thumbnail, int>
    {
        public int Convert(Thumbnail source, int destination, ResolutionContext context)
        {
            return source.Id;
        }
    }
}
