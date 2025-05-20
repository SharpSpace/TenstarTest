using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public sealed class User
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    public int Id { get; set; }

    [Required]
    [MinLength(9, ErrorMessage = "Password must be longer than 8 characters.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{9,}$",
        ErrorMessage = "Password must contain at least one upper case letter, one lower case letter, one digit, and one special character."
    )]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;
}
