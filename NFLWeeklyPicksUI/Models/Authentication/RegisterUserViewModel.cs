using System.ComponentModel.DataAnnotations;

namespace NFLWeeklyPicksUI.Models.Authentication
{
    public class RegisterUserViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public IList<string> Roles { get; set; } = new List<string>() { "User" };
    }
}