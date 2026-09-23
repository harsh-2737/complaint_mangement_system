using complaint_mangement_system.Models;

namespace complaint_mangement_system.ViewModels
{
    public class StaffCategoryViewModel
    {
        public int categoryid { get; set; }
        public string categoryname { get; set; }
        public List<User> staffs { get; set; } = new List<User>();
    }
}