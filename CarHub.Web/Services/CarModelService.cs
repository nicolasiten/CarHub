using CarHub.Web.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHub.Web.Services
{
    public class CarModelService : ICarModelService
    {
        public IEnumerable<string> ValidateCarImages(IEnumerable<string> images)
        {
            List<string> errors = new List<string>();

            if (!images.Any() || images.First() == string.Empty)
            {
                errors.Add("Add at least one image");
            }
            else
            {
                foreach (var image in images)
                {
                    dynamic imageObject = JObject.Parse(image);

                    string type = ((string)imageObject.type).ToLower();
                    if (!type.Contains("jpg") && !type.Contains("jpeg") && !type.Contains("png"))
                    {
                        errors.Add("Images must be of type jpg or png.");
                    }
                    else if (int.TryParse(imageObject.size.Value.ToString(), out int imageSize) && imageSize > 5000000)
                    {
                        errors.Add("Images must be smaller than 5MB.");
                    }
                }
            }

            return errors;
        }
    }
}
