using complaint_mangement_system.Data;
using complaint_mangement_system.Models;
using complaint_mangement_system.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace complaint_mangement_system.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public IActionResult Dashboard()
        {
            int userid = GetUserId();

            var complaints = _context.Complaints
                .Where(c => c.userid == userid)
                .ToList();

            var user = _context.Users
                .FirstOrDefault(u => u.userid == userid);

            ViewBag.UserName = user?.name;

            ViewBag.TotalComplaints =
                complaints.Count;

            ViewBag.PendingComplaints =
                complaints.Count(c => c.status == "Pending");

            ViewBag.ResolvedComplaints =
                complaints.Count(c => c.status == "Resolved");

            List<UserComplaintViewModel> complaintViewModels =
                new List<UserComplaintViewModel>();

            foreach (var complaint in complaints)
            {
                var category = _context.Categories
                    .FirstOrDefault(c =>
                        c.categoryid == complaint.categoryid);

                complaintViewModels.Add(
                    new UserComplaintViewModel
                    {
                        complaintid = complaint.complaintid,
                        categoryid = complaint.categoryid,
                        categoryname = category?.categoryname,
                        userid = complaint.userid,
                        complaintname = complaint.complaintname,
                        description = complaint.description,
                        priority = complaint.priority,
                        status = complaint.status
                    }
                );
            }

            return View(complaintViewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadCategories();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            UserComplaintViewModel c1)
        {
            int userid = GetUserId();

            ModelState.Remove("status");
            ModelState.Remove("categoryname");

            if (!ModelState.IsValid)
            {
                LoadCategories(c1.categoryid);

                return View(c1);
            }

            var category = _context.Categories
                .FirstOrDefault(c =>
                    c.categoryid == c1.categoryid);

            if (category == null)
            {
                ModelState.AddModelError(
                    "categoryid",
                    "Please select a valid category."
                );

                LoadCategories(c1.categoryid);

                return View(c1);
            }

            Complaint complaint = new Complaint
            {
                categoryid = c1.categoryid,
                userid = userid,
                complaintname = c1.complaintname,
                description = c1.description,
                priority = c1.priority,
                status = "Pending"
            };

            _context.Complaints.Add(complaint);

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            int userid = GetUserId();

            var user = _context.Users
                .FirstOrDefault(u =>
                    u.userid == userid &&
                    u.role == "User");

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(User user)
        {
            ModelState.Remove("password");
            ModelState.Remove("role");

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            int userid = GetUserId();

            var existingUser = _context.Users
                .FirstOrDefault(u =>
                    u.userid == userid &&
                    u.role == "User");

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.name = user.name;
            existingUser.email = user.email;
            existingUser.country_code = user.country_code;
            existingUser.phone_no = user.phone_no;

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        public IActionResult Complaints()
        {
            int userid = GetUserId();

            var complaints = _context.Complaints
                .Where(c => c.userid == userid)
                .ToList();

            List<UserComplaintViewModel> complaintViewModels =
                new List<UserComplaintViewModel>();

            foreach (var complaint in complaints)
            {
                var category = _context.Categories
                    .FirstOrDefault(c =>
                        c.categoryid == complaint.categoryid);

                UserComplaintViewModel viewModel =
                    new UserComplaintViewModel
                    {
                        complaintid = complaint.complaintid,
                        categoryid = complaint.categoryid,
                        categoryname = category?.categoryname,
                        userid = complaint.userid,
                        complaintname = complaint.complaintname,
                        description = complaint.description,
                        priority = complaint.priority,
                        status = complaint.status
                    };

                complaintViewModels.Add(viewModel);
            }

            return View(complaintViewModels);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            int userid = GetUserId();

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == id &&
                    c.userid == userid);

            if (complaint == null)
            {
                return NotFound();
            }

            var category = _context.Categories
                .FirstOrDefault(c =>
                    c.categoryid == complaint.categoryid);

            UserComplaintViewModel viewModel =
                new UserComplaintViewModel
                {
                    complaintid = complaint.complaintid,
                    categoryid = complaint.categoryid,
                    categoryname = category?.categoryname,
                    userid = complaint.userid,
                    complaintname = complaint.complaintname,
                    description = complaint.description,
                    priority = complaint.priority,
                    status = complaint.status
                };

            LoadCategories(complaint.categoryid);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(
            UserComplaintViewModel c1)
        {
            int userid = GetUserId();

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == c1.complaintid &&
                    c.userid == userid);

            if (complaint == null)
            {
                return NotFound();
            }

            ModelState.Remove("status");
            ModelState.Remove("categoryname");

            if (!ModelState.IsValid)
            {
                LoadCategories(c1.categoryid);

                return View(c1);
            }

            var category = _context.Categories
                .FirstOrDefault(c =>
                    c.categoryid == c1.categoryid);

            if (category == null)
            {
                ModelState.AddModelError(
                    "categoryid",
                    "Please select a valid category."
                );

                LoadCategories(c1.categoryid);

                return View(c1);
            }

            complaint.categoryid = c1.categoryid;
            complaint.complaintname = c1.complaintname;
            complaint.description = c1.description;
            complaint.priority = c1.priority;

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult ConfirmDelete(int id)
        {
            int userid = GetUserId();

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == id &&
                    c.userid == userid);

            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            int userid = GetUserId();

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == id &&
                    c.userid == userid);

            if (complaint == null)
            {
                return NotFound();
            }

            _context.Complaints.Remove(complaint);

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
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

        private void LoadCategories(
            int? selectedCategoryId = null)
        {
            ViewBag.Categories = new SelectList(
                _context.Categories.ToList(),
                "categoryid",
                "categoryname",
                selectedCategoryId
            );
        }
    }
}