using System.ComponentModel.DataAnnotations;

namespace NZWalkRevise.Models.DTOs
{
    public class RegionDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Max length should be 50.")]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public string Description { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
