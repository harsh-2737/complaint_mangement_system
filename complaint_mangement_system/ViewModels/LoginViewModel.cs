using System.ComponentModel.DataAnnotations;

namespace complaint_mangement_system.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email is required.")]
        [EmailAddress(ErrorMessage ="Enter valid email.")]
        [Display(Name ="Email")]
        public string email { get; set; }

        [Required(ErrorMessage ="Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string password { get; set; }

    }
}
