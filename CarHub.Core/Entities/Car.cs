using System;
using System.Collections.Generic;
using System.Text;

namespace CarHub.Core.Entities
{
    public class Car : BaseEntity
    {
        public Car()
        {
            Repairs = new HashSet<Repair>();
            Images = new HashSet<Image>();
        }

        public string Vin { get; set; }

        public int Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Trim { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime LotDate { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }

        public DateTime? SaleDate { get; set; }

        public bool ShowCase { get; set; }

        public virtual Thumbnail ThumbnailImage { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Repair> Repairs { get; set; }
    }
}
