using NZWalkRevise.Models.DTOs;

namespace NZWalkRevise.Repositories.Interface
{
    public interface IWalk
    {
        Task<string> GetAllWalk(string? filterBy = null, string? filterQuery = null, string? orderBy = null, bool isAsc = true);
        Task<string> GetWalkById(Guid walkId);
        Task<string> CreateWalk(AddUpdateWalkDto addWalk);
        Task<string> UpdateWalk(Guid walkId, AddUpdateWalkDto updateWalk);
        Task<string> DeleteWalk(Guid walkId);

    }
}
