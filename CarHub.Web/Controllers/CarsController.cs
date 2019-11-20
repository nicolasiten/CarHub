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

        public CarsController(ICarModelService carModelService, IAsyncRepository<Image> imageRepository, IAsyncRepository<Thumbnail> thumbnailRepository)
        {
            _carModelService = carModelService;
            _imageRepository = imageRepository;
            _thumbnailRepository = thumbnailRepository;
        }

        public IActionResult Index()
        {
            return View();
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

            return RedirectToAction("EditCar").WithSuccess("Success", "Successfully saved Car.");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCar(CarModel carModel)
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

            return RedirectToAction("Overview").WithSuccess("Success", "Successfully saved Car.");
        }

        [Authorize]
        public async Task<IActionResult> Overview()
        {
            var carModels = await _carModelService.GetCarModelsAsync();
            return View(carModels);
        }

        [Authorize]
        public async Task<IActionResult> RemoveImage(int id)
        {
            await _imageRepository.DeleteAsync(id);

            return Json("Ok");
        }

        [Authorize]
        public async Task<IActionResult> SetAsThumbnail(int thumbnailId, int imageId)
        {
            var thumbnail = await _thumbnailRepository.GetByIdAsync(thumbnailId);
            thumbnail.File = (await _imageRepository.GetByIdAsync(imageId)).File;
            await _thumbnailRepository.UpdateAsync(thumbnail);

            return Json("Ok");
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
    }
}