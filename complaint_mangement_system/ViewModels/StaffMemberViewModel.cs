using System.ComponentModel.DataAnnotations;

namespace complaint_mangement_system.ViewModels
{
    public class StaffMemberViewModel
    {
        public int userid { get; set; }

        [Display(Name = "Name")]
        public string name { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Country Code")]
        public string country_code { get; set; }

        [Display(Name = "Phone Number")]
        public string phone_no { get; set; }
    }
}