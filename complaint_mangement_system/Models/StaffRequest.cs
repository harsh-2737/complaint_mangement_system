
using System.ComponentModel.DataAnnotations;

namespace complaint_mangement_system.Models
{
    public class StaffRequest
    {
        [Key]
        public int requestid { get; set; }

        [Required]
        public int userid { get; set; }

        [Required]
        public int categoryid { get; set; }

        [Required]
        public string status { get; set; }
    }
}
