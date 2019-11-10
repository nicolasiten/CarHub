using System;
using System.Collections.Generic;
using System.Text;

namespace CarHub.Core.Entities
{
    public class Repair : BaseEntity
    {
        public int? CarFk { get; set; }

        public string RepairDescription { get; set; }

        public decimal RepairCost { get; set; }

        public virtual Car Car { get; set; }
    }
}
