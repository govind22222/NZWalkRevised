using System.ComponentModel.DataAnnotations;

namespace NZWalkRevise.Models.DTOs
{
    public class AddRegionDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Max length should be 50.")]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "Max length should be 200.")]
        public string Description { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
