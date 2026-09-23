using System.ComponentModel.DataAnnotations;

namespace complaint_mangement_system.ViewModels
{
    public class StaffComplaintViewModel
    {
        public int complaintid { get; set; }

        [Required(ErrorMessage = "Please select category.")]
        public int categoryid { get; set; }

        [Required]
        public int userid { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [Display(Name = "User Name")]
        public string username { get; set; }

        [Required(ErrorMessage = "Please enter your complaint title.")]
        [StringLength(100, ErrorMessage = "Your complaint title characters must under 100.")]
        [Display(Name = "Complaint Title")]
        public string complaintname { get; set; }

        [Required(ErrorMessage = "Please enter Description about your complaint")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Enter Description between 10 to 200 characters.")]
        [Display(Name = "Description")]
        public string description { get; set; }


        [Required(ErrorMessage = "Status is required.")]
        [Display(Name = "Status")]
        public string status { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [Display(Name = "Category")]
        public string categoryname { get; set; }


        [Required(ErrorMessage = "Country Code must selected.")]
        [Display(Name = "Country Code")]
        public string country_code { get; set; }

        [Required(ErrorMessage = "Phone Number field must required.")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Your Phone Number must contain exctly 10 digit.")]
        public string phone_no { get; set; }
    }
}