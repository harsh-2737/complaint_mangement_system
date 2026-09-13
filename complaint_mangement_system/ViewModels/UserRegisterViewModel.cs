using System.ComponentModel.DataAnnotations;

namespace complaint_mangement_system.ViewModels
{
    public class UserRegisterViewModel
    {

        [Required(ErrorMessage = "Name field must required.")]
        [Display(Name = "Full Name")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 charcaters.")]
        public string name { get; set; }

        [Required(ErrorMessage = "Email field must required.")]
        [EmailAddress(ErrorMessage = "Enter vaild email address.")]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password field must required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Confrim your password.")]
        [Compare("password", ErrorMessage = "password and confirm password must match.")]
        [Display(Name = "Confirm Password")]
        public string confirm_password { get; set; }

        [Required(ErrorMessage = "Country Code must selected.")]
        [Display(Name = "Country Code")]
        public string country_code { get; set; }

        [Required(ErrorMessage = "Phone Number field must required.")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Your Phone Number must contain exctly 10 digit.")]
        public string phone_no { get; set; }

        [Required(ErrorMessage = "Role must be selected.")]
        [Display(Name = "Role")]
        public string role { get; set; }
    }
}
