using System.ComponentModel.DataAnnotations;

namespace NZWalkRevise.Models.DTOs
{
    public class UpdateRegionDto
    {

        [Required]
        [MaxLength(50, ErrorMessage = "Max length should be 50.")]
        public string Name { get; set; }
        [Required]
        [Length(2, 5, ErrorMessage = "Length should be between 2 to 5.")]
        public string Code { get; set; }
        [MaxLength(200, ErrorMessage = "Max length should be 200.")]
        public string Description { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
