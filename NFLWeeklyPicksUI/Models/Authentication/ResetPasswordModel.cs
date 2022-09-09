using System.ComponentModel.DataAnnotations;

namespace NFLWeeklyPicksUI.Models.Authentication;

public class ResetPasswordModel
{
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }

    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmationPassword { get; set; }

    public string Email { get; set; }
    public string Token { get; set; }
}