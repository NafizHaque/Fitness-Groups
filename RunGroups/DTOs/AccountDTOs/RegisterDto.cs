using System.ComponentModel.DataAnnotations;

namespace RunGroups.DTOs.AccountDTOs
{
    public class RegisterDto
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
