using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHub.Web.Interfaces
{
    public interface ICarModelService
    {
        IEnumerable<string> ValidateCarImages(IEnumerable<string> images);
    }
}
