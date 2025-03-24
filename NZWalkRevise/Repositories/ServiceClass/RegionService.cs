using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NZWalkRevise.Database;
using NZWalkRevise.Models.DomainModels;
using NZWalkRevise.Models.DTOs;
using NZWalkRevise.Repositories.Interface;

namespace NZWalkRevise.Repositories.ServiceClass
{

    public class RegionService : IRegion
    {
        private readonly NZWalkDbContext _db;
        private readonly ResponseModelDto responseModel = new();
        public RegionService(NZWalkDbContext db)
        {
            _db = db;
        }

        public async Task<string> GetAllRegions()
        {
            var regionList = await _db.Regions.ToListAsync();

            if (regionList is not null)
            {
                responseModel.Data = JsonConvert.SerializeObject(regionList);
                responseModel.IsSuccess = true;
                responseModel.SuccessMessage = "Region Fetched Successfully !!";
            }
            else
            {
                responseModel.IsSuccess = false;
                responseModel.ErrorMessage = "No Region Found !!";
            }
            return JsonConvert.SerializeObject(responseModel);
        }

        public async Task<string> GetRegionById(Guid regionId)
        {
            var region = await _db.Regions.FirstOrDefaultAsync(x => x.Id == regionId);
            if (region is not null)
            {
                responseModel.IsSuccess = true;
                responseModel.Data = JsonConvert.SerializeObject(region);
                responseModel.SuccessMessage = "Region Fetched Successfully !!";
            }
            else
            {
                responseModel.IsSuccess = false;
                responseModel.ErrorMessage = "Region not Found !!";
            }
            return JsonConvert.SerializeObject(responseModel);
        }

        public async Task<string> AddRegion(AddRegionDto addRegionDto)
        {
            var regionModel = new Region()
            {
                Name = addRegionDto.Name,
                Code = addRegionDto.Code,
                Description = addRegionDto.Description,
                RegionImageUrl = addRegionDto.RegionImageUrl
            };
            using var transaction = await _db.Database.BeginTransactionAsync();
            await _db.Regions.AddAsync(regionModel);
            if (Convert.ToBoolean(await _db.SaveChangesAsync()))
            {
                await transaction.CommitAsync();
                responseModel.IsSuccess = true;
                responseModel.Data = JsonConvert.SerializeObject(regionModel);
                responseModel.SuccessMessage = "Region Added Successfully !!";
            }
            else
            {
                responseModel.IsSuccess = false;
                responseModel.ErrorMessage = "Region not Added !!";
            }
            return JsonConvert.SerializeObject(responseModel);
        }

        public async Task<string> UpdateRegion(UpdateRegionDto updateRegionDto, Guid id)
        {
            var regionData = await _db.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionData is not null)
            {
                regionData.Code = updateRegionDto.Code;
                regionData.Name = updateRegionDto.Name;
                regionData.Description = updateRegionDto.Description;
                regionData.RegionImageUrl = updateRegionDto.RegionImageUrl;

                using var transaction = await _db.Database.BeginTransactionAsync();
                _db.Regions.Update(regionData);
                if (Convert.ToBoolean(await _db.SaveChangesAsync()))
                {
                    await transaction.CommitAsync();

                    responseModel.IsSuccess = true;
                    responseModel.SuccessMessage = "Region Updated Successfully !!";
                    responseModel.Data = JsonConvert.SerializeObject(regionData);
                }
                else
                {
                    responseModel.IsSuccess = false;
                    responseModel.ErrorMessage = "Region not Updated !!";
                }
            }
            else
            {
                responseModel.IsSuccess = false;
                responseModel.ErrorMessage = "Region not Found !!";
            }
            return JsonConvert.SerializeObject(responseModel);
        }

        public async Task<string> DeleteRegion(Guid regionId)
        {
            var regionData = await _db.Regions.FirstOrDefaultAsync(x => x.Id == regionId);
            if (regionData is not null)
            {
                using var transaction = await _db.Database.BeginTransactionAsync();
                _db.Regions.Remove(regionData);
                if (Convert.ToBoolean(await _db.SaveChangesAsync()))
                {
                    await transaction.CommitAsync();
                    responseModel.IsSuccess = true;
                    responseModel.SuccessMessage = "Region Deleted Successfully !!";
                    responseModel.Data = JsonConvert.SerializeObject(regionData);
                    return JsonConvert.SerializeObject(responseModel);
                }
                else
                {
                    responseModel.IsSuccess = false;
                    responseModel.SuccessMessage = "Region Not Deleted !!";
                }
            }
            else
            {
                responseModel.IsSuccess = false;
                responseModel.ErrorMessage = "Region not Found !!";
            }
            return JsonConvert.SerializeObject(responseModel);
        }

    }
}
