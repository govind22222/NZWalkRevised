using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NZWalkRevise.Database;
using NZWalkRevise.ModelFilters;
using NZWalkRevise.Models.DomainModels;
using NZWalkRevise.Models.DTOs;
using NZWalkRevise.Repositories.Interface;

namespace NZWalkRevise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalkDbContext _db;
        private readonly IRegion _region;
        private readonly IMapper _autoMapper;
        private readonly ResponseModelDto responseModel = new();

        public RegionsController(NZWalkDbContext db, IRegion region, IMapper autoMapper)
        {
            _db = db;
            _region = region;
            _autoMapper = autoMapper;
        }

        [HttpGet]
        [Route("GetAllRegions")]
        public async Task<IActionResult> GetAllRegion([FromQuery] string? filterBy, [FromQuery] string? filterValue, [FromQuery] string? orderBy, bool isAsc = true, int pageNumber = 1, int pageSize = 100)
        {
            var regionResponse = JsonConvert.DeserializeObject<ResponseModelDto>(await _region.GetAllRegions(filterBy, filterValue, orderBy, isAsc, pageNumber, pageSize));
            if (regionResponse is null)
            {
                return BadRequest("Some Error Occured Region List not retrived !!");
            }
            if (regionResponse.IsSuccess is false)
            {
                return BadRequest(regionResponse.ErrorMessage);
            }
            if (string.IsNullOrEmpty(regionResponse?.Data) || regionResponse?.Data.Count() == 0)
            {
                return NotFound("Region not Found !!");
            }
            var regionList = JsonConvert.DeserializeObject<List<Region>>(regionResponse.Data);
            //Mapping Region domain model list to RegionDto list
            var regionDtoList = _autoMapper.Map<List<RegionDTO>>(regionList);
            return Ok(regionDtoList);
        }

        [HttpGet]
        [Route("GetRegionById/{id:guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            var regionResponse = JsonConvert.DeserializeObject<ResponseModelDto>(await _region.GetRegionById(id));
            if (regionResponse is null)
            {
                return StatusCode(500, $" Some Error Occured Region with Id:'{id}' not found.");
            }
            if (regionResponse?.IsSuccess is not true)
            {
                return NotFound(regionResponse?.ErrorMessage);
            }
            if (string.IsNullOrEmpty(regionResponse?.Data) || regionResponse?.Data?.Count() == 0)
            {
                return BadRequest($"Region data with Id:'{id}' retrived but having some error to display. !!");
            }
            var region = JsonConvert.DeserializeObject<Region>(regionResponse.Data);
            return Ok(_autoMapper.Map<RegionDTO>(region));
        }

        [HttpPost]
        [Route("CreateNewRegion")]
        [FilterValidateModelAttributes]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionDto addRegion)
        {
            if (addRegion is null)
            {
                return BadRequest("Region Data should not Empty.");
            }
            var regionResponse = JsonConvert.DeserializeObject<ResponseModelDto>(await _region.AddRegion(addRegion));
            if (regionResponse is null)
            {
                return StatusCode(500, $"Some Error Occured Region:'{addRegion.Name}' not Added !!");
            }
            if (regionResponse?.IsSuccess is not true)
            {
                return BadRequest(regionResponse?.ErrorMessage);
            }
            if (regionResponse?.Data is null || regionResponse?.Data?.Count() == 0)
            {
                return BadRequest("Region added but data not Returned to Swagger !!");
            }
            var region = JsonConvert.DeserializeObject<Region>(regionResponse.Data);
            var regionDto = _autoMapper.Map<RegionDTO>(region);
            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("UpdateRegion/{id:guid}")]
        [FilterValidateModelAttributes]
        public async Task<IActionResult> UpdateRegionById([FromBody] UpdateRegionDto updateRegion, [FromRoute] Guid id)
        {
            if (updateRegion is null)
            {
                return BadRequest("Region Data should not Empty.");
            }
            var updateResponse = JsonConvert.DeserializeObject<ResponseModelDto>(await _region.UpdateRegion(updateRegion, id));
            if (updateResponse is null)
            {
                return StatusCode(500, $"Error Occured Region with id:'{id}' not updated.");
            }
            if (updateResponse?.IsSuccess is not true)
            {
                return BadRequest(updateResponse?.ErrorMessage);
            }
            if (string.IsNullOrEmpty(updateResponse?.Data))
            {
                return NotFound("Region updated but data is missing to return.");
            }
            var updatedRegion = JsonConvert.DeserializeObject<Region>(updateResponse.Data);
            return Ok(_autoMapper.Map<RegionDTO>(updatedRegion));
        }

        [HttpDelete]
        [Route("DeleteRegion/{deleteId:guid}")]
        public async Task<IActionResult> DeleteRegionById([FromRoute] Guid deleteId)
        {
            var deleteResponse = JsonConvert.DeserializeObject<ResponseModelDto>(await _region.DeleteRegion(deleteId));
            if (deleteResponse is null)
            {
                return BadRequest(deleteResponse?.ErrorMessage ?? $"Some error occured Region with id:'{deleteId}' not deleted.");
            }
            if (deleteResponse.IsSuccess is not true)
            {
                return BadRequest(deleteResponse?.ErrorMessage);
            }
            if (string.IsNullOrEmpty(deleteResponse?.Data))
            {
                return NotFound($"Region Deleted but data is missing to return.");
            }
            var deletedRegion = JsonConvert.DeserializeObject<Region>(deleteResponse.Data);
            var regionData = _autoMapper.Map<RegionDTO>(deletedRegion);
            return Ok($"Region '{regionData.Name}' Deleted Successfully.");
        }

    }
}
