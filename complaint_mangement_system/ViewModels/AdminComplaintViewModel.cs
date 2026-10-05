using System.ComponentModel.DataAnnotations;

namespace complaint_mangement_system.ViewModels
{
    public class AdminComplaintViewModel
    {
        [Required]
        public int complaintid { get; set; }

        [Required]
        public int categoryid { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string? categoryname { get; set; }

        [Required]
        public int userid { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string? username { get; set; }

        [Required(ErrorMessage = "Complaint title is required.")]
        [StringLength(100, ErrorMessage = "Complaint title cannot exceed 100 characters.")]
        public string? complaintname { get; set; }

        [Required(ErrorMessage = "Complaint description is required.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 200 characters.")]
        public string? description { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string? status { get; set; }
    }
}