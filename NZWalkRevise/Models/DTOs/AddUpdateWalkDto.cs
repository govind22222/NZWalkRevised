using System.ComponentModel.DataAnnotations;

namespace NZWalkRevise.Models.DTOs
{
    public class AddUpdateWalkDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Max length should be 50.")]
        public string Name { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "Max length should be 200.")]
        public string Description { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Range should be between 1-100Km.")]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid? DifficultyId { get; set; }
        [Required]
        public Guid? RegionId { get; set; }
    }
}
