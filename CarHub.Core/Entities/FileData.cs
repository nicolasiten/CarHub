using System;
using System.Collections.Generic;
using System.Text;

namespace CarHub.Core.Entities
{
    public class FileData : BaseEntity
    {
        public int? CarFk { get; set; }

        public byte[] File { get; set; }

        public virtual Car Car { get; set; }
    }
}
