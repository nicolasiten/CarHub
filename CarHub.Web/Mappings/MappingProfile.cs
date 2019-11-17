using AutoMapper;
using CarHub.Core.Entities;
using CarHub.Web.Models;

namespace CarHub.Web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarModel>()
                .ReverseMap();
        }
    }
}
