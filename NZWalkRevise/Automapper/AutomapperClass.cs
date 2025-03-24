using AutoMapper;
using NZWalkRevise.Models.DomainModels;
using NZWalkRevise.Models.DTOs;

namespace NZWalkRevise.Automapper
{
    public class AutomapperClass : Profile
    {
        public AutomapperClass()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<AddRegionDto, Region>().ReverseMap();
            CreateMap<UpdateRegionDto, Region>().ReverseMap();
            CreateMap<WalkDto, Walk>().ReverseMap();
            CreateMap<AddUpdateWalkDto, Walk>().ReverseMap();
        }
    }
}
