using System.ComponentModel.DataAnnotations;

namespace API_Foreignkey.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [EmailAddress(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9_]+@(gmail\.com)$", ErrorMessage = "Email is not a valid Gmail address")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Phone number must be 10 digits long.")]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string role { get; set; }

    }
    public class UserVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string role { get; set; }
        public string Token { get; set; }

    }

}
