using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CarHub.Core.Entities;
using CarHub.Core.Interfaces;
using CarHub.Web.Interfaces;
using CarHub.Web.Models;
using CarHub.Web.Utils.Alerts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarHub.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarModelService _carModelService;
        private readonly IAsyncRepository<Image> _imageRepository;
        private readonly IAsyncRepository<Thumbnail> _thumbnailRepository;
        private readonly IImageService _imageService;
        private readonly ICarService _carService;

        public CarsController(
            ICarModelService carModelService, 
            IAsyncRepository<Image> imageRepository, 
            IAsyncRepository<Thumbnail> thumbnailRepository,
            IImageService imageService,
            ICarService carService)
        {
            _carModelService = carModelService;
            _imageRepository = imageRepository;
            _thumbnailRepository = thumbnailRepository;
            _imageService = imageService;
            _carService = carService;
        }

        public async Task<IActionResult> Index()
        {
            var carModels = (await _carModelService.GetCarModelsAsync())
                .Where(cm => cm.ShowCase || cm.SaleDate == null)
                .OrderByDescending(cm => cm.LotDate).ToList();

            return View(carModels);
        }

        public async Task<IActionResult> CarDetails(int id)
        {
            var carModel = await _carModelService.GetCarModelByIdAsync(id);

            if (carModel == null)
            {
                return NotFound($"Car with {id} not found!");
            }

            return View(carModel);
        }

        [Authorize]
        public IActionResult AddCar()
        {
            return View(new CarModel());
        }

        [Authorize]
        public async Task<IActionResult> EditCar(int id)
        {
            var car = await _carModelService.GetCarModelByIdAsync(id);

            if (car == null)
            {
                return NotFound($"Car with id {id} not found.");
            }

            return View(car);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditCar(CarModel carModel)
        {
            return await SaveCar(carModel, "EditCar");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCar(CarModel carModel)
        {
            return await SaveCar(carModel, "Overview");
        }

        [Authorize]
        public async Task<IActionResult> Overview()
        {
            var carModels = (await _carModelService.GetCarModelsAsync())
                .OrderBy(cm => cm.SaleDate != null)
                .ThenByDescending(cm => cm.SaleDate)
                .ThenByDescending(cm => cm.Id);
            return View(carModels);
        }

        [Authorize]
        public async Task<IActionResult> RemoveImage(int id)
        {
            await _imageRepository.DeleteAsync(id);

            return Json("Ok");
        }

        [Authorize]
        public async Task<IActionResult> SetAsThumbnail(int imageId, int carId)
        {
            await _imageService.SetNewThumbnailAsync(imageId, carId);

            return Json("Ok");
        }

        [Authorize]
        public async Task<IActionResult> SetSalesDate(int id, DateTime date)
        {
            if (id == null || id == 0)
            {
                return StatusCode(400, "invalid car id!");
            }

            if (date == null)
            {
                return StatusCode(400, "invalid date!");
            }

            await _carService.SetSalesDateAsync(id, date);

            return Ok("Successfully set sales date");
        }

        public async Task<FileStreamResult> GetImage(int id)
        {
            return new FileStreamResult(new MemoryStream((await _imageRepository.GetByIdAsync(id)).File), "image/jpg");
        }

        public async Task<FileStreamResult> GetThumbnail(int id)
        {
            return new FileStreamResult(new MemoryStream((await _thumbnailRepository.GetByIdAsync(id)).File), "image/jpg");
        }

        public IActionResult AddRepair()
        {
            return PartialView("_RepairRow", new RepairModel());
        }

        private async Task<IActionResult> SaveCar(CarModel carModel, string redirectAction)
        {
            IEnumerable<string> imageErrors = _carModelService.ValidateCarImages(Request.Form["images"]);

            foreach (string error in imageErrors)
            {
                ModelState.AddModelError("", error);
            }

            if (!ModelState.IsValid)
            {
                return View(carModel);
            }

            await _carModelService.SaveCarModelAsync(carModel, Request.Form["images"]);

            return RedirectToAction(redirectAction).WithSuccess("Success", "Successfully saved Car.");
        }
    }
}