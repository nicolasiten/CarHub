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

            if (images != null)
            { 
                foreach (var image in images)
                {
                    if (!string.IsNullOrEmpty(image))
                    {
                        dynamic imageObject = JObject.Parse(image);
                        
                        if (!string.IsNullOrEmpty((string)imageObject.type) && imageObject.size != null && !string.IsNullOrEmpty(imageObject.size.Value.ToString()))
                        {
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

            foreach (string image in images)
            {
                if (!string.IsNullOrEmpty(image))
                {
                    dynamic imageObject = JObject.Parse(image);

                    if (imageObject.data != null && !string.IsNullOrEmpty(imageObject.data.Value))
                    {
                        byte[] file = Convert.FromBase64String(imageObject.data.Value);

                        if (car.Id == 0 && car.ThumbnailImage == null)
                        {
                            car.ThumbnailImage = new Thumbnail { File = file };
                        }

                        car.Images.Add(new Image
                        {
                            File = file
                        });
                    }
                }
            }

            if (car.Id == 0)
            {
                await _carRepository.AddAsync(car);
            }
            else
            {
                await _carRepository.UpdateAsync(car);
            }
        }

        public async Task<IEnumerable<CarModel>> GetCarModelsAsync()
        {
            return _mapper.Map<List<Car>, List<CarModel>>(
                (await _carRepository.GetAllAsync(includeProperties: "Images,ThumbnailImage,Repairs")).ToList());
        }

        public async Task<CarModel> GetCarModelByIdAsync(int id)
        {
            return _mapper.Map<Car, CarModel>(
                (await _carRepository.GetAllAsync(includeProperties: "Images,ThumbnailImage,Repairs")).SingleOrDefault(c => c.Id == id));
        }
    }
}
