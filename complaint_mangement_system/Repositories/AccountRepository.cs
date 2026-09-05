using complaint_mangement_system.Data;
using complaint_mangement_system.Models;
using complaint_mangement_system.Repositories;
using complaint_mangement_system.ViewModels;

namespace complaint_mangement_system.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Users
                .FirstOrDefault(u => u.email == email);
        }

        public User? Login(string email, string password)
        {
            return _context.Users
                .FirstOrDefault(u =>
                    u.email == email &&
                    u.password == password);
        }

        public User Register(RegisterViewModel model)
        {
            var user = new User
            {
                name = model.name,
                email = model.email,
                password = model.password,
                country_code = model.country_code,
                phone_no = model.phone_no,
                role = "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
    }
}