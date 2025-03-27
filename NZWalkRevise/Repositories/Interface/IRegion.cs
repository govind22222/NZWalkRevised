using NZWalkRevise.Models.DomainModels;
using NZWalkRevise.Models.DTOs;

namespace NZWalkRevise.Repositories.Interface
{
    public interface IRegion
    {
        public Task<string> GetAllRegions(string? filterBy, string? filterValue, string? orderBy, bool isAsc, int pageNumber = 1, int pageSize = 100);
        public Task<string> GetRegionById(Guid regionId);
        public Task<string> AddRegion(AddRegionDto addRegionDto);
        public Task<string> UpdateRegion(UpdateRegionDto updateRegionDto, Guid regionId);
        public Task<string> DeleteRegion(Guid regionId);
    }
}
