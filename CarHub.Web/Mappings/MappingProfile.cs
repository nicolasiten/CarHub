using AutoMapper;
using CarHub.Core.Entities;
using CarHub.Web.Models;
using System.Collections.Generic;

namespace CarHub.Web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarModel>()
                .ForMember(cm => cm.Images, c => c.MapFrom(car => car.Images))
                .ForMember(cm => cm.Thumbnail, c => c.MapFrom(car => car.ThumbnailImage));

            CreateMap<CarModel, Car>()
                .ForMember(c => c.ThumbnailImage, cm => cm.Ignore())
                .ForMember(c => c.Images, cm => cm.Ignore());

            CreateMap<IEnumerable<Image>, IEnumerable<string>>()
                .ConvertUsing(new ImageBase64Resolver());

            CreateMap<Thumbnail, string>()
                .ConvertUsing(new ThumbnailBase64Resolver());
        }
    }
}
