using System.ComponentModel.DataAnnotations;

namespace complaint_mangement_system.Models
{
    public class Comment
    {
        [Key]
        public int commentid { get; set; }

        [Required]
        public int userid { get; set; }

        [Required]
        public int complaintid { get; set; }

        [Required(ErrorMessage ="Comment is required.")]
        [StringLength(100,ErrorMessage="Comment characters must be under 100.")]
        [Display(Name ="Comment")]
        public string comment {  get; set; }
        
        //created at
    }
}
