using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarHub.Web.Models
{
    public class RepairModel
    {
        public int Id { get; set; }

        public int? CarFk { get; set; }

        [Required]
        public string RepairDescription { get; set; }

        [Required]
        [Range(0, 10000000)]
        public decimal RepairCost { get; set; }

        public bool Delete { get; set; }
    }
}
