using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHub.Core.Interfaces
{
    public interface ICarService
    {
        Task SetSalesDateAsync(int id, DateTime date);
    }
}
