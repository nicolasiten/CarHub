using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarHub.Web.Interfaces;
using CarHub.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarHub.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarModelService _carModelService;

        public CarsController(ICarModelService carModelService)
        {
            _carModelService = carModelService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult AddCar()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddCar(CarModel carModel)
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

            return View();
        }
    }
}