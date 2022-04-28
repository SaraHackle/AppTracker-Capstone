using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppTrackerMVC.Auth.Models
{
    public class Registration
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}