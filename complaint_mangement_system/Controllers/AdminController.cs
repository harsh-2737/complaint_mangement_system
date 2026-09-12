using complaint_mangement_system.Data;
using complaint_mangement_system.Models;
using Microsoft.AspNetCore.Mvc;

namespace complaint_mangement_system.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("DashBoard");
        }

        public IActionResult DashBoard()
        {
            return View();
        }

        public IActionResult Users()
        {
            var users = _context.Users.Where(u => u.role == "User").ToList();

            return View(users);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(User user)
        {
            user.role = "User";

            ModelState.Remove("role");

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var existingUser =
                _context.Users.FirstOrDefault(u => u.email == user.email && u.role == "User");

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "email",
                    "Email already exists."
                );

                return View(user);
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Users");
        }

        [HttpGet]
        public IActionResult ConfirmDeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.userid == id && u.role == "User");

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.userid == id && u.role == "User");

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("Users");
        }


        public IActionResult Staffs()
        {
            var staffs = _context.Users.Where(u => u.role == "Staff").ToList();

            return View(staffs);
        }

        [HttpGet]
        public IActionResult CreateStaff()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStaff(User staff)
        {
            staff.role = "Staff";

            ModelState.Remove("role");

            if (!ModelState.IsValid)
            {
                return View(staff);
            }

            var existingStaff =
                _context.Users.FirstOrDefault(u => u.email == staff.email && u.role == "Staff");

            if (existingStaff != null)
            {
                ModelState.AddModelError(
                    "email",
                    "Email already exists."
                );

                return View(staff);
            }

            _context.Users.Add(staff);
            _context.SaveChanges();

            return RedirectToAction("Staffs");
        }

        [HttpGet]
        public IActionResult ConfirmDeleteStaff(int id)
        {
            var staff = _context.Users.FirstOrDefault(u => u.userid == id && u.role == "Staff");

            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteStaff(int id)
        {
            var staff = _context.Users.FirstOrDefault(u => u.userid == id && u.role == "Staff");

            if (staff == null)
            {
                return NotFound();
            }

            _context.Users.Remove(staff);
            _context.SaveChanges();

            return RedirectToAction("Staffs");
        }
    }
}
