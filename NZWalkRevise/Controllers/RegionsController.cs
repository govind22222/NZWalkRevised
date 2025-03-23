using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalkRevise.Database;

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
            if (regionList is not null)
            {
                return Ok(regionList);
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
        [Route("GeRegionById/{id:guid}")]
        public async Task<IActionResult> GeRegionById([FromRoute] Guid id)
        {
            var region = await _db.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (region is not null)
            {
                return Ok(region);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
