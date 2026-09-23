using System.ComponentModel.DataAnnotations;

namespace complaint_mangement_system.ViewModels
{
    public class UserComplaintViewModel
    {
        public int complaintid { get; set; }

        [Required(ErrorMessage = "Please select category.")]
        public int categoryid { get; set; }

        [Required]
        public int userid { get; set; }

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
    }
}