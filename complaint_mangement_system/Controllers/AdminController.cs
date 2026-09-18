using System.Security.Claims;
using complaint_mangement_system.Data;
using complaint_mangement_system.Models;
using complaint_mangement_system.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace complaint_mangement_system.Controllers
{
    [Authorize(Roles = "Admin")]
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
            var users = _context.Users
                .Where(u => u.role == "User")
                .ToList();

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
                _context.Users.FirstOrDefault(
                    u => u.email == user.email
                );

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
            var user = _context.Users
                .FirstOrDefault(u =>
                    u.userid == id &&
                    u.role == "User");

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
            var user = _context.Users
                .FirstOrDefault(u =>
                    u.userid == id &&
                    u.role == "User");

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
            var staffs = _context.Users
                .Where(u => u.role == "Staff")
                .ToList();

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
                _context.Users.FirstOrDefault(
                    u => u.email == staff.email
                );

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
            var staff = _context.Users
                .FirstOrDefault(u =>
                    u.userid == id &&
                    u.role == "Staff");

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
            var staff = _context.Users
                .FirstOrDefault(u =>
                    u.userid == id &&
                    u.role == "Staff");

            if (staff == null)
            {
                return NotFound();
            }

            _context.Users.Remove(staff);
            _context.SaveChanges();

            return RedirectToAction("Staffs");
        }

        [HttpGet]
        public IActionResult Categories()
        {
            var categories =
                _context.Categories.ToList();

            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var existingCategory =
                _context.Categories.FirstOrDefault(
                    c => c.categoryname ==
                         category.categoryname
                );

            if (existingCategory != null)
            {
                ModelState.AddModelError(
                    "categoryname",
                    "Category already exists."
                );

                return View(category);
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("Categories");
        }

        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var category =
                _context.Categories.FirstOrDefault(
                    c => c.categoryid == id
                );

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(
            Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var existingCategory =
                _context.Categories.FirstOrDefault(
                    c =>
                        c.categoryname ==
                        category.categoryname &&
                        c.categoryid !=
                        category.categoryid
                );

            if (existingCategory != null)
            {
                ModelState.AddModelError(
                    "categoryname",
                    "Category already exists."
                );

                return View(category);
            }

            var oldCategory =
                _context.Categories.FirstOrDefault(
                    c => c.categoryid ==
                         category.categoryid
                );

            if (oldCategory == null)
            {
                return NotFound();
            }

            oldCategory.categoryname =
                category.categoryname;

            oldCategory.description =
                category.description;

            _context.SaveChanges();

            return RedirectToAction("Categories");
        }

        [HttpGet]
        public IActionResult ConfirmDeleteCategory(
            int id)
        {
            var category =
                _context.Categories.FirstOrDefault(
                    c => c.categoryid == id
                );

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int id)
        {
            var category =
                _context.Categories.FirstOrDefault(
                    c => c.categoryid == id
                );

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Categories");
        }

        public IActionResult StaffRequests()
        {
            var requests = _context.StaffRequests
                .Where(r => r.status == "Pending")
                .ToList();

            var staffRequests =
                requests.Select(r =>
                    new StaffRequestViewModel
                    {
                        staffid = r.userid,

                        name = _context.Users
                            .Where(u =>
                                u.userid == r.userid)
                            .Select(u => u.name)
                            .FirstOrDefault(),

                        categoryname =
                            _context.Categories
                                .Where(c =>
                                    c.categoryid ==
                                    r.categoryid)
                                .Select(c =>
                                    c.categoryname)
                                .FirstOrDefault(),

                        status = r.status
                    }
                ).ToList();

            return View(staffRequests);
        }

        [HttpGet]
        public IActionResult ApproveStaff(
            int staffid)
        {
            var request =
                _context.StaffRequests.FirstOrDefault(
                    r =>
                        r.userid == staffid &&
                        r.status == "Pending"
                );

            if (request == null)
            {
                return NotFound();
            }

            var staff =
                _context.Users.FirstOrDefault(
                    u => u.userid == staffid
                );

            var category =
                _context.Categories.FirstOrDefault(
                    c =>
                        c.categoryid ==
                        request.categoryid
                );

            var model =
                new StaffRequestViewModel
                {
                    staffid = staffid,
                    name = staff.name,
                    categoryname =
                        category.categoryname,
                    status = request.status
                };

            return View(model);
        }

        [HttpPost]
        public IActionResult ConfirmApproveStaff(
            int staffid)
        {
            var request =
                _context.StaffRequests.FirstOrDefault(
                    r =>
                        r.userid == staffid &&
                        r.status == "Pending"
                );

            if (request == null)
            {
                return NotFound();
            }

            request.status = "Approved";

            _context.SaveChanges();

            return RedirectToAction("StaffRequests");
        }

        [HttpGet]
        public IActionResult RejectStaff(
            int staffid)
        {
            var request =
                _context.StaffRequests.FirstOrDefault(
                    r =>
                        r.userid == staffid &&
                        r.status == "Pending"
                );

            if (request == null)
            {
                return NotFound();
            }

            var staff =
                _context.Users.FirstOrDefault(
                    u => u.userid == staffid
                );

            var category =
                _context.Categories.FirstOrDefault(
                    c =>
                        c.categoryid ==
                        request.categoryid
                );

            var model =
                new StaffRequestViewModel
                {
                    staffid = staffid,
                    name = staff.name,
                    categoryname =
                        category.categoryname,
                    status = request.status
                };

            return View(model);
        }

        [HttpPost]
        public IActionResult ConfirmRejectStaff(
            int staffid)
        {
            var request =
                _context.StaffRequests.FirstOrDefault(
                    r =>
                        r.userid == staffid &&
                        r.status == "Pending"
                );

            if (request == null)
            {
                return NotFound();
            }

            request.status = "Rejected";

            _context.SaveChanges();

            return RedirectToAction("StaffRequests");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            int userid = GetUserId();

            var admin = _context.Users
                .FirstOrDefault(u =>
                    u.userid == userid &&
                    u.role == "Admin");

            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(User admin)
        {
            ModelState.Remove("password");
            ModelState.Remove("role");

            if (!ModelState.IsValid)
            {
                return View(admin);
            }

            int userid = GetUserId();

            var existingAdmin = _context.Users
                .FirstOrDefault(u =>
                    u.userid == userid &&
                    u.role == "Admin");

            if (existingAdmin == null)
            {
                return NotFound();
            }

            existingAdmin.name = admin.name;
            existingAdmin.email = admin.email;
            existingAdmin.country_code =
                admin.country_code;
            existingAdmin.phone_no =
                admin.phone_no;

            _context.SaveChanges();

            return RedirectToAction("DashBoard");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                "Cookies"
            );

            return RedirectToAction(
                "Login",
                "Account"
            );
        }

        private int GetUserId()
        {
            return int.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier
                )
            );
        }
    }
}