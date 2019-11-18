﻿using AutoMapper;
using CarHub.Core.Entities;
using CarHub.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHub.Web.Mappings
{
    public class ImageIdResolver : ITypeConverter<IEnumerable<Image>, IEnumerable<int>>
    {
        public IEnumerable<int> Convert(IEnumerable<Image> source, IEnumerable<int> destination, ResolutionContext context)
        {
            return source.Select(s => s.Id);
        }
    }
}
