using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NZWalkRevise.Automapper;
using NZWalkRevise.Database;
using NZWalkRevise.Models.DomainModels;
using NZWalkRevise.Models.DTOs;
using NZWalkRevise.Repositories.Interface;

namespace NZWalkRevise.Repositories.ServiceClass
{

    public class RegionService : IRegion
    {
        private readonly NZWalkDbContext _db;
        private readonly IMapper _autoMappper;
        private readonly ResponseModelDto responseModel = new();
        public RegionService(NZWalkDbContext db, IMapper autoMappper)
        {
            _db = db;
            _autoMappper = autoMappper;
        }

        public async Task<string> GetAllRegions(string? filterBy, string? filterValue, string? orderby, bool isAsc = true, int pageNumber = 1, int pageSize = 100)
        {
            //var regionList = await _db.Regions.AsNoTracking().ToListAsync();
            var regionData = _db.Regions.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(filterBy) && !string.IsNullOrEmpty(filterValue))
            {
                switch (filterValue.ToLower())
                {
                    case "name":
                        regionData = regionData.Where(r => EF.Functions.Like(r.Name, $"%{filterValue}%"));
                        regionData = isAsc ? regionData.OrderBy(r => r.Name) : regionData.OrderByDescending(r => r.Name);
                        break;

                    case "code":
                        regionData = regionData.Where(r => EF.Functions.Like(r.Code, $"%{filterValue}%"));
                        break;

                    case "deccription":
                        regionData = regionData.Where(r => EF.Functions.Like(r.Description, $"%{filterValue}%"));
                        break;
                    default:
                        regionData = regionData.Where(r => EF.Functions.Like(r.Name, $"%{filterValue}%"));
                        break;
                }
            }
            var skipCount = (pageNumber - 1) * pageSize;
            var regionList = await regionData.Skip(skipCount).Take(pageSize).ToListAsync();
            if (regionList is not null && regionList.Count() != 0)
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
            var region = await _db.Regions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == regionId);
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
            var regionModel = _autoMappper.Map<Region>(addRegionDto);
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
                regionData.Name = updateRegionDto.Name;
                regionData.Code = updateRegionDto.Code;
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
