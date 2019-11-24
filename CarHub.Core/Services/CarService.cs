using CarHub.Core.Entities;
using CarHub.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHub.Core.Services
{
    public class CarService : ICarService
    {
        private readonly IAsyncRepository<Car> _carRepository;

        public CarService(IAsyncRepository<Car> carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task SetSalesDateAsync(int id, DateTime date)
        {
            var car = await _carRepository.GetByIdAsync(id);

            if (car != null)
            {
                car.SaleDate = date;
                await _carRepository.UpdateAsync(car);
            }
        }
    }
}
