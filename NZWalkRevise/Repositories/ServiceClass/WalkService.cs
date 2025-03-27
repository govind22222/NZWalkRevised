using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NZWalkRevise.Database;
using NZWalkRevise.Models.DomainModels;
using NZWalkRevise.Models.DTOs;
using NZWalkRevise.Repositories.Interface;

namespace NZWalkRevise.Repositories.ServiceClass
{
    public class WalkService : IWalk
    {
        private readonly NZWalkDbContext _db;
        private readonly IMapper _autoMapper;
        private readonly ResponseModelDto responseModel = new();
        public WalkService(NZWalkDbContext db, IMapper autoMapper)
        {
            _db = db;
            _autoMapper = autoMapper;
        }

        public async Task<string> GetAllWalk(string? filteBy, string? filterQuery, string? orderBy, bool isAsc = true, int pageNumber = 1, int pageSize = 100)
        {
            //var walkData = await _db.Walks.AsNoTracking().Include(w => w.Difficulty).Include(w => w.Region).ToListAsync();
            var walkData = _db.Walks.Include(w => w.Difficulty).Include(w => w.Region).AsQueryable();

            if (!string.IsNullOrEmpty(filteBy) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filteBy.ToLower())
                {
                    case "name":
                        walkData = walkData.Where(w => EF.Functions.Like(w.Name, $"%{filterQuery}%"));
                        walkData = isAsc ? walkData.OrderBy(w => w.Name) : walkData.OrderByDescending(w => w.Name);
                        break;
                    case "description":
                        walkData = walkData.Where(w => EF.Functions.Like(w.Description, $"%{filterQuery}%"));
                        walkData = isAsc ? walkData.OrderBy(w => w.Description) : walkData.OrderByDescending(w => w.Description);
                        break;
                    case "lengthinkm":
                        walkData = walkData.Where(w => EF.Functions.Like(w.LengthInKm.ToString(), $"%{filterQuery}%"));
                        walkData = isAsc ? walkData.OrderBy(w => w.LengthInKm) : walkData.OrderByDescending(w => w.LengthInKm);
                        break;
                    default:
                        walkData = walkData.Where(w => EF.Functions.Like(w.Name.ToString(), $"%{filterQuery}%"));
                        walkData = isAsc ? walkData.OrderBy(w => w.Name) : walkData.OrderByDescending(w => w.Name);
                        break;
                }
            }

            var skipResults = (pageNumber - 1) * pageSize;
            var walkList = await walkData.Skip(skipResults).Take(pageSize).ToListAsync();
            if (walkList is not null && walkList.Count() != 0)
            {
                responseModel.IsSuccess = true;
                responseModel.SuccessMessage = "Walk list retrieved Successfully !!";
                responseModel.Data = JsonConvert.SerializeObject(walkList);
            }
            else
            {
                responseModel.IsSuccess = false;
                responseModel.ErrorMessage = "Walk data not Found !!";
            }
            return JsonConvert.SerializeObject(responseModel);
        }

        public async Task<string> GetWalkById(Guid walkId)
        {
            var walkResponse = await _db.Walks.AsNoTracking().Include(w => w.Difficulty).Include(w => w.Region).FirstOrDefaultAsync(w => w.Id == walkId);
            if (walkResponse is not null)
            {
                responseModel.IsSuccess = true;
                responseModel.SuccessMessage = $"Walk Id:'{walkId}' retrived successfully !!";
                responseModel.Data = JsonConvert.SerializeObject(walkResponse);
            }
            else
            {
                responseModel.IsSuccess = false;
                responseModel.ErrorMessage = $"Walk Id:'{walkId}' not found !!";
            }
            return JsonConvert.SerializeObject(responseModel);
        }

        public async Task<string> CreateWalk(AddUpdateWalkDto addWalk)
        {
            var walkModel = _autoMapper.Map<Walk>(addWalk);
            using var transaction = await _db.Database.BeginTransactionAsync();
            await _db.Walks.AddAsync(walkModel);
            if (Convert.ToBoolean(await _db.SaveChangesAsync()))
            {
                responseModel.IsSuccess = true;
                responseModel.SuccessMessage = "Walk Added Successfully !!";
                responseModel.Data = JsonConvert.SerializeObject(walkModel);
                await transaction.CommitAsync();
            }
            else
            {
                responseModel.IsSuccess = false;
                responseModel.ErrorMessage = "Walk not Added !!";
                await transaction.RollbackAsync();
            }
            return JsonConvert.SerializeObject(responseModel);
        }

        public async Task<string> UpdateWalk(Guid walkId, AddUpdateWalkDto updateWalkDto)
        {
            var walkModel = await _db.Walks.FirstOrDefaultAsync(w => w.Id == walkId);
            if (walkModel is null)
            {
                responseModel.IsSuccess = false;
                responseModel.ErrorMessage = $"Walk Id:'{walkId}' not found !!";
            }
            else
            {
                walkModel.Name = updateWalkDto.Name;
                walkModel.Description = updateWalkDto.Description;
                walkModel.LengthInKm = updateWalkDto.LengthInKm;
                walkModel.WalkImageUrl = updateWalkDto.WalkImageUrl;
                walkModel.DifficultyId = updateWalkDto.DifficultyId;
                walkModel.RegionId = updateWalkDto.RegionId;

                using var transaction = await _db.Database.BeginTransactionAsync();
                _db.Update(walkModel);
                if (Convert.ToBoolean(await _db.SaveChangesAsync()))
                {
                    responseModel.IsSuccess = true;
                    responseModel.SuccessMessage = "Walk data update Successfully !!";
                    responseModel.Data = JsonConvert.SerializeObject(walkModel);
                    await transaction.CommitAsync();
                }
                else
                {
                    responseModel.IsSuccess = false;
                    responseModel.ErrorMessage = $"Walk data for Id:'{walkId}'not updated.";
                    await transaction.RollbackAsync();
                }
            }
            return JsonConvert.SerializeObject(responseModel);
        }

        public async Task<string> DeleteWalk(Guid walkId)
        {
            var deleteModel = await _db.Walks.FirstOrDefaultAsync(w => w.Id == walkId);
            if (deleteModel is null)
            {
                responseModel.IsSuccess = false;
                responseModel.ErrorMessage = $"Walk with Id:'{walkId} not found !!'";
            }
            else
            {
                using var transaction = await _db.Database.BeginTransactionAsync();
                _db.Remove(deleteModel);
                if (Convert.ToBoolean(await _db.SaveChangesAsync()))
                {
                    responseModel.IsSuccess = true;
                    responseModel.SuccessMessage = $"Walk with Id:'{walkId}' deleted successfully !!";
                    responseModel.Data = JsonConvert.SerializeObject(deleteModel);
                    await transaction.CommitAsync();
                }
                else
                {
                    responseModel.IsSuccess = false;
                    responseModel.ErrorMessage = $"Walk Id:'{walkId}' not delete !!";
                    await transaction.RollbackAsync();
                }
            }
            return JsonConvert.SerializeObject(responseModel);
        }

    }
}
