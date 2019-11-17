using AutoMapper;
using CarHub.Core.Entities;
using CarHub.Core.Interfaces;
using CarHub.Web.Interfaces;
using CarHub.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHub.Web.Services
{
    public class CarModelService : ICarModelService
    {
        private readonly IAsyncRepository<Car> _carRepository;
        private readonly IMapper _mapper;

        public CarModelService(IAsyncRepository<Car> carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }

        public IEnumerable<string> ValidateCarImages(IEnumerable<string> images)
        {
            List<string> errors = new List<string>();

            if (images == null || !images.Any() || images.First() == string.Empty)
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

        public async Task SaveCarModelAsync(CarModel carModel, IEnumerable<string> images)
        {
            if (carModel == null)
            {
                throw new ArgumentNullException(nameof(carModel));
            }

            if (images == null)
            {
                throw new ArgumentNullException(nameof(images));
            }

            Car car = _mapper.Map<CarModel, Car>(carModel);

            foreach (var image in images)
            {
                dynamic imageObject = JObject.Parse(image);

                if (car.ThumbnailImage == null)
                {
                    car.ThumbnailImage = new Thumbnail { File = Convert.FromBase64String(imageObject.data.Value) };
                }

                car.Images.Add(new Image
                {
                    File = Convert.FromBase64String(imageObject.data.Value)
                });
            }


            await _carRepository.AddAsync(car);
        }
    }
}
