using System.ComponentModel.DataAnnotations;

namespace RunGroups.DTOs.AccountDTOs
{
    public class LoginDto
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage ="Email Required")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
