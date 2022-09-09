using System.ComponentModel.DataAnnotations;

namespace NFLWeeklyPicksAPI.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}