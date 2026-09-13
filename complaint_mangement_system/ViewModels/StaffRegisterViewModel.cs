
using System.ComponentModel.DataAnnotations;

namespace complaint_mangement_system.ViewModels
{
    public enum StaffStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class StaffRegisterViewModel
    {
        [Required(ErrorMessage = "Name field must required.")]
        [Display(Name = "Full Name")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string name { get; set; }

        [Required(ErrorMessage = "Email field must required.")]
        [EmailAddress(ErrorMessage = "Enter valid email address.")]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password field must required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Country Code must selected.")]
        [Display(Name = "Country Code")]
        public string country_code { get; set; }

        [Required(ErrorMessage = "Phone Number field must required.")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Your Phone Number must contain exactly 10 digits.")]
        public string phone_no { get; set; }

        [Required(ErrorMessage = "Please select category.")]
        [Display(Name = "Category")]
        public int categoryid { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [Display(Name = "Status")]
        public StaffStatus status { get; set; }

        [Required(ErrorMessage = "Role must be selected.")]
        [Display(Name = "Role")]
        public string role { get; set; }
    }
}