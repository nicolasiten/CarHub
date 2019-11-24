using CarHub.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace CarHub.Web.Utils
{
    public static class ThumbnailHelpers
    {
        public static string ResolveThumbnailSrc(this IUrlHelper url, CarModel carModel)
        {
            if(carModel.ThumbnailId.HasValue && carModel.ThumbnailId.Value > 0)
            {
                
                return url.Action("GetThumbnail", "Cars", new { id = carModel.ThumbnailId });
            }
            else
            {
                return "/images/no-image-available.jpg";
            }
        }
    }
}
