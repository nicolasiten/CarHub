using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarHub.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace CarHub.Web.Controllers
{
    public class CarsController : Controller
    {
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
            if (!ModelState.IsValid)
            {
                return View(carModel);
            }

            var images = Request.Form["images"];
            if (!images.Any() || images.First() == string.Empty)
            {
                ModelState.AddModelError("", "Add at least one image");
                return View();
            }

            // TODO validate Image size and type

            return View();
        }
    }
}