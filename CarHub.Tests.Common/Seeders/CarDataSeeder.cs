using CarHub.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarHub.Tests.Common.Seeders
{
    public class CarDataSeeder
    {
        public static IEnumerable<Car> GetEntities()
        {
            return new[]
            {
                new Car
                {
                    Description = "Description",
                    Kilometers = 100,
                    LotDate = new DateTime(2019, 12, 1),
                    PurchaseDate = new DateTime(2019, 11, 30),
                    Make = "Make",
                    Model = "Model",
                    Trim = "Trim",
                    TransmissionType = Core.Enums.TransmissionType.Automatic,
                    Vin = "11111111111111111",
                    PurchasePrice = 1000,
                    SellingPrice = 2000,
                    ShowCase = true,
                    Year = 2002,
                    ThumbnailImage = new Thumbnail
                    {
                        ImageType = "png",
                        File = Convert.FromBase64String(FileDataSeeder.GetBase64Images().First())
                    },
                    Repairs = new List<Repair>
                    {
                        new Repair
                        {
                            RepairDescription = "Description",
                            RepairCost = 100
                        }
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImageType = "png",
                            File = Convert.FromBase64String(FileDataSeeder.GetBase64Images().First())
                        }
                    }
                },
                new Car
                {
                    Description = "Description1",
                    Kilometers = 200,
                    LotDate = new DateTime(2019, 11, 12),
                    PurchaseDate = new DateTime(2019, 11, 10),
                    Make = "Make1",
                    Model = "Model1",
                    Trim = "Trim1",
                    TransmissionType = Core.Enums.TransmissionType.Manual,
                    Vin = "22222222222222222",
                    PurchasePrice = 5000,
                    SellingPrice = 7000,
                    ShowCase = false,
                    Year = 2012
                }
            };
        }
    }
}
