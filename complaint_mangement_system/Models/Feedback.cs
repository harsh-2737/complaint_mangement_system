using System.ComponentModel.DataAnnotations;

namespace complaint_mangement_system.Models
{
    public class Feedback
    {
        [Key]
        public int feedbackid { get; set; }

        [Required]
        public int complaintid { get; set; }

        [Required]
        public int userid { get; set; }

        [Range(1.0,5.0,ErrorMessage ="Rating must be between 1.0 to 5.0.")]
        [Display(Name ="Rating")]
        public double rate { get; set; }

        [StringLength(100,ErrorMessage=" Comment character cannot exceed 100 characters.")]
        [Display(Name ="Comment")]
        public string comment { get; set; }
        //created at
    }
}
