using complaint_mangement_system.Models;
using complaint_mangement_system.ViewModels;

namespace complaint_mangement_system.Repositories
{
    public interface IAccountRepository
    {
        User? Login(string email, string password);

        User? GetUserByEmail(string email);

        User Register(RegisterViewModel model);
    }
}