using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be minimum 3 characters long")]
        [MaxLength(3, ErrorMessage = "Code has to be maximum 3 characters long")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name can be maximum 100 characters long")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
