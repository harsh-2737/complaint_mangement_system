using System.ComponentModel.DataAnnotations;

namespace complaint_mangement_system.ViewModels
{
    public class AdminComplaintViewModel
    {
        public int complaintid { get; set; }

        [Required(ErrorMessage = "Please select category.")]
        [Display(Name = "Category")]
        public int categoryid { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int userid { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [Display(Name = "User Name")]
        public string username { get; set; }

        [Display(Name = "Staff ID")]
        public int? staffid { get; set; }

        [Display(Name = "Staff Name")]
        public string staffname { get; set; }

        [Required(ErrorMessage = "Please enter your complaint title.")]
        [StringLength(100, ErrorMessage = "Your complaint title cannot exceed 100 characters.")]
        [Display(Name = "Complaint Title")]
        public string complaintname { get; set; }

        [Required(ErrorMessage = "Please enter description about your complaint.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Enter description between 10 and 200 characters.")]
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