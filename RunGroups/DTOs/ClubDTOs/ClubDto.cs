using RunGroups.Data.Enums;
using RunGroups.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroups.DTOs.ClubDTOs
{
    public class ClubDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Address Address { get; set; }
        public IFormFile? Image { get; set; }
        public ClubCategory ClubCategory { get; set; }
        public string AppUserId { get; set; }


    }
}
