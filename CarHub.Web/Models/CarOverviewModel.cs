using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHub.Web.Models
{
    public class CarOverviewModel
    {
        public CarOverviewModel()
        {
            CarsForSale = new List<CarModel>();
            CarsShowcase = new List<CarModel>();
            CarsRecentlySold = new List<CarModel>();
        }

        public IEnumerable<CarModel> CarsForSale { get; set; }

        public IEnumerable<CarModel> CarsShowcase { get; set; }

        public IEnumerable<CarModel> CarsRecentlySold { get; set; }
    }
}
