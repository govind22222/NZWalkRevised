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
    public class WalksController : ControllerBase
    {
        private readonly NZWalkDbContext _db;
        private readonly IWalk _walk;
        private readonly IMapper _autoMapper;
        private readonly ResponseModelDto responseModel = new();

        public WalksController(NZWalkDbContext db, IWalk walk, IMapper autoMapper)
        {
            _db = db;
            _walk = walk;
            _autoMapper = autoMapper;
        }

        [HttpGet]
        [Route("GetAllWalks")]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterBy, [FromQuery] string? filterQuery, [FromQuery] string? orderBy, [FromQuery] bool isAsc = true)
        {
            var walkResponse = JsonConvert.DeserializeObject<ResponseModelDto>(await _walk.GetAllWalk(filterBy, filterQuery, orderBy, isAsc));
            if (walkResponse is null)
            {
                return BadRequest("No WalkData found, Some Error Occured !!");
            }
            if (walkResponse?.IsSuccess is false)
            {
                return BadRequest(walkResponse?.ErrorMessage);
            }
            var walkList = JsonConvert.DeserializeObject<List<Walk>>(walkResponse.Data);
            if (walkList is null || walkList.Count() == 0)
            {
                return BadRequest("Walk List Fetched but data missing to return !!");
            }
            return Ok(_autoMapper.Map<List<WalkDto>>(walkList));
        }

        [HttpGet]
        [Route("GetWalkById/{walkId:guid}")]
        public async Task<IActionResult> GetWalkUsingId(Guid walkId)
        {
            var walkResponse = JsonConvert.DeserializeObject<ResponseModelDto>(await _walk.GetWalkById(walkId));
            if (walkResponse is null)
            {
                return BadRequest("Walk data notfound Error occured !!");
            }
            if (walkResponse?.IsSuccess is false)
            {
                return BadRequest(walkResponse?.ErrorMessage);
            }
            var walkModel = JsonConvert.DeserializeObject<Walk>(walkResponse.Data);
            if (walkModel is null)
            {
                return BadRequest($"Walk Id:'{walkId}' but data is missing to return!!");
            }
            return Ok(_autoMapper.Map<WalkDto>(walkModel));
        }

        [HttpPost]
        [Route("AddWalk")]
        [FilterValidateModelAttributes]
        public async Task<IActionResult> CreateWalk([FromBody] AddUpdateWalkDto addWalkDto)
        {
            if (addWalkDto is null)
            {
                return BadRequest("Walk data should not be Empty !!");
            }
            var walkResponse = JsonConvert.DeserializeObject<ResponseModelDto>(await _walk.CreateWalk(addWalkDto));
            if (walkResponse is null)
            {
                return StatusCode(500, "Walk not Created, Some Error occured !!");
            }
            if (walkResponse?.IsSuccess is false)
            {
                return BadRequest(walkResponse?.ErrorMessage);
            }
            var walk = JsonConvert.DeserializeObject<Walk>(walkResponse.Data);
            if (walk is null)
            {
                return BadRequest("Walk Created but data is missing to return!!");
            }
            var WalkDto = _autoMapper.Map<WalkDto>(walk);
            return CreatedAtAction(nameof(GetWalkUsingId), new { walkId = WalkDto.Id }, WalkDto);
        }

        [HttpPut]
        [Route("UpdateWalk/{walkId:guid}")]
        [FilterValidateModelAttributes]
        public async Task<IActionResult> UpdateWalkData([FromBody] AddUpdateWalkDto updateWalkDto, Guid walkId)
        {
            if (updateWalkDto is null)
            {
                return BadRequest("Walk data should not be empty !!");
            }
            var updateResponse = JsonConvert.DeserializeObject<ResponseModelDto>(await _walk.UpdateWalk(walkId, updateWalkDto));
            if (updateResponse is null)
            {
                return BadRequest($"Walk Id:'{walkId}' not updated, Some error Occured !!");

            }
            if (updateResponse?.IsSuccess is false)
            {
                return BadRequest(updateResponse?.ErrorMessage);
            }

            var updatedModel = JsonConvert.DeserializeObject(updateResponse.Data);
            if (updatedModel is null)
            {
                return BadRequest($" Walk Id:'{walkId}' updated but data not retrived.");
            }
            return Ok(_autoMapper.Map<AddUpdateWalkDto>(updatedModel));
        }

        [HttpDelete]
        [Route("DeleteWalk/{walkId:guid}")]
        public async Task<IActionResult> DeleteRegionById([FromRoute] Guid walkId)
        {
            var deleteResponse = JsonConvert.DeserializeObject<ResponseModelDto>(await _walk.DeleteWalk(walkId));
            if (deleteResponse is null)
            {
                return BadRequest($"Walk Id:'{walkId}' not deleted, Some error Occured !!");

            }
            if (deleteResponse?.IsSuccess is false)
            {
                return BadRequest(deleteResponse?.ErrorMessage);
            }

            var deletedModel = JsonConvert.DeserializeObject(deleteResponse.Data);
            if (deletedModel is null)
            {
                return BadRequest($" Walk Id:'{walkId}' Deleted but data not retrived.");
            }
            return Ok(_autoMapper.Map<WalkDto>(deletedModel));
        }


    }
}
