using NZWalkRevise.Models.DTOs;

namespace NZWalkRevise.Repositories.Interface
{
    public interface IWalk
    {
        Task<string> GetAllWalk();
        Task<string> GetWalkById(Guid walkId);
        Task<string> CreateWalk(AddUpdateWalkDto addWalk);
        Task<string> UpdateWalk(Guid walkId, AddUpdateWalkDto updateWalk);
        Task<string> DeleteWalk(Guid walkId);

    }
}
