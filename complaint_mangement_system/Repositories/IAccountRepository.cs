using complaint_mangement_system.Models;
using complaint_mangement_system.ViewModels;

namespace complaint_mangement_system.Repositories
{
    public interface IAccountRepository
    {
        User? Login(string email, string password);

        User? GetUserByEmail(string email);

        User RegisterAsUser(UserRegisterViewModel model);
        User RegisterAsStaff(StaffRegisterViewModel model);

        List<Category> GetCategories();

        StaffRequest CreateStaffRequest(int userid, int categoryid);

        StaffRequest? GetStaffRequest(int userid);
    }
}