using CarHub.Web.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarHub.Web.Models
{
    public class CarModel
    {
        public CarModel()
        {
            RepairModels = new List<RepairModel>();
            ImageIds = new List<int>();
            PurchaseDate = DateTime.Now;
            LotDate = DateTime.Now;
        }

        public int? Id { get; set; }

        [Required]
        [MinLength(17)]
        [MaxLength(17)]        
        public string Vin { get; set; }

        [Required]
        [YearValidation(1990)]
        public int Year { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Trim { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DateEqualOrGreaterThan(nameof(PurchaseDate))]
        public DateTime LotDate { get; set; }

        [Required]
        [Range(0, 10000000)]
        public decimal PurchasePrice { get; set; }

        [Required]
        [Range(0, 10000000)]
        public decimal SellingPrice { get; set; }

        public DateTime? SaleDate { get; set; }

        [Required]
        public bool ShowCase { get; set; }

        public IEnumerable<RepairModel> RepairModels { get; set; }

        public IList<int> ImageIds { get; set; }

        public int ThumbnailId { get; set; }
    }
}
