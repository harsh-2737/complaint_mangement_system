using System.ComponentModel.DataAnnotations;

namespace complaint_mangement_system.Models
{
    public class Category
    {
        [Key]
        public int categoryid {  get; set; }

        [Required(ErrorMessage ="Category name is required.")]
        [StringLength(25,ErrorMessage ="Category name cannot exceed 25 characters.")]
        [Display(Name ="Category Name")]
        public string categoryname { get; set; }

        [StringLength(100,ErrorMessage ="Description cannot exceed 100 cahracters.")]
        [Display(Name ="Description")]
        public string description { get; set; }

    }
}
