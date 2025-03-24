using NZWalkRevise.Models.DomainModels;
using NZWalkRevise.Models.DTOs;

namespace NZWalkRevise.Repositories.Interface
{
    public interface IRegion
    {
        public Task<string> GetAllRegions();
        public Task<string> GetRegionById(Guid regionId);
        public Task<string> AddRegion(AddRegionDto addRegionDto);
        public Task<string> UpdateRegion(UpdateRegionDto updateRegionDto, Guid regionId);
        public Task<string> DeleteRegion(Guid regionId);
    }
}
