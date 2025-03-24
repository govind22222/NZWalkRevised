using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalkRevise.Database;
using NZWalkRevise.Models.DomainModels;
using NZWalkRevise.Models.DTOs;

namespace NZWalkRevise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalkDbContext _db;
        public RegionsController(NZWalkDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegion()
        {
            var regionList = await _db.Regions.ToListAsync();

            var regionDto = new List<RegionDTO>();

            foreach (var item in regionList)
            {
                regionDto.Add(new RegionDTO()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Code = item.Code,
                    Description = item.Description,
                    RegionImageUrl = item.RegionImageUrl
                });

            }
            if (regionDto is not null)
            {
                return Ok(regionDto);
            }
            else
            {
                return NotFound();
            }
            #region "Commented Code"
            //var regionsList = new List<Region>() { 
            //new Region()
            //{
            //    Id= Guid.NewGuid(),
            //    Name="Aukland Region",
            //    Code="AUK",
            //    Description="This is Aukland Region.",
            //    RegionImageUrl="https://unsplash.com/photos/a-bunch-of-balloons-that-are-shaped-like-email-7NT4EDSI5Ok"
            //},
            //new Region()
            //{
            //    Id= Guid.NewGuid(),
            //    Name="Willington Region",
            //    Code="WTN",
            //    Description="This is Willington Region.",
            //    RegionImageUrl="https://unsplash.com/photos/two-people-sitting-on-a-couch-playing-video-games-xoT1rt09NEI"
            //}
            //};
            #endregion 
        }

        [HttpGet]
        [Route("GetRegionById/{id:guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            var region = await _db.Regions.FirstOrDefaultAsync(r => r.Id == id);
            var regionDto = new RegionDTO()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                Description = region.Description,
                RegionImageUrl = region.RegionImageUrl

            };
            if (region is not null)
            {
                return Ok(regionDto);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        [Route("CreateNewRegion")]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionDto region)
        {
            var addRegion = new Region()
            {
                Name = region.Name,
                Code = region.Code,
                Description = region.Description,
                RegionImageUrl = region.RegionImageUrl
            };
            using var transaction = await _db.Database.BeginTransactionAsync();
            var addResponse = await _db.Regions.AddAsync(addRegion);
            if (Convert.ToBoolean(await _db.SaveChangesAsync()))
            {
                await transaction.CommitAsync();
                var regionDto = new RegionDTO()
                {
                    Id = addRegion.Id,
                    Name = addRegion.Name,
                    Code = addRegion.Code,
                    Description = addRegion.Description,
                    RegionImageUrl = addRegion.RegionImageUrl
                };
                return CreatedAtAction(nameof(GetRegionById), new { id = addRegion.Id }, regionDto);
            }
            else
            {
                await transaction.RollbackAsync();
                return BadRequest();
            }
        }


        [HttpPut]
        [Route("UpdateRegion/{id:guid}")]
        public async Task<IActionResult> UpdateRegionById([FromBody] UpdateRegionDto updateRegion, [FromRoute] Guid id)
        {
            var regionData = await _db.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionData is not null)
            {
                regionData.Code = updateRegion.Code;
                regionData.Name = updateRegion.Name;
                regionData.Description = updateRegion.Description;
                regionData.RegionImageUrl = updateRegion.RegionImageUrl;

                using var transaction = await _db.Database.BeginTransactionAsync();
                _db.Regions.Update(regionData);
                if (Convert.ToBoolean(await _db.SaveChangesAsync()))
                {
                    await transaction.CommitAsync();
                    RegionDTO regionDTO = new RegionDTO()
                    {
                        Id = regionData.Id,
                        Name = regionData.Name,
                        Code = regionData.Code,
                        Description = regionData.Description,
                        RegionImageUrl = regionData.RegionImageUrl

                    };
                    return Ok(regionDTO);
                }
                else
                {
                    await transaction.RollbackAsync();
                    return BadRequest("REgion not Updated.");
                }
            }
            else
            {
                return NotFound("Region not found.");
            }
        }

        [HttpDelete]
        [Route("DeleteRegion/{deleteId:guid}")]
        public async Task<IActionResult> DeleteRegionById([FromRoute] Guid deleteId)
        {
            var regionData = await _db.Regions.FirstOrDefaultAsync(x => x.Id == deleteId);
            if (regionData is not null)
            {
                using var transaction = await _db.Database.BeginTransactionAsync();
                _db.Regions.Remove(regionData);
                if (Convert.ToBoolean(await _db.SaveChangesAsync()))
                {
                    await transaction.CommitAsync();
                    return Ok($"Region '{regionData.Name}' Deleted Successfully.");
                }
                else
                {
                    await transaction.RollbackAsync();
                    return BadRequest("Region not Deleted.");
                }
            }
            else
            {
                return NotFound($"Region with Id:'{deleteId}' is not found.");
            }

        }

    }
}
