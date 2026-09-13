using complaint_mangement_system.Data;
using complaint_mangement_system.Models;
using complaint_mangement_system.ViewModels;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace complaint_mangement_system.Controllers
{
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StaffController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================================================
        // INDEX
        // ====================================================

        public IActionResult Index()
        {
            int? userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("Dashboard");
        }

        // ====================================================
        // DASHBOARD
        // ====================================================

        public IActionResult Dashboard()
        {
            int? userid = HttpContext.Session.GetInt32("userid");
            int? categoryid = HttpContext.Session.GetInt32("categoryid");

            if (userid == null || categoryid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var complaints = _context.Complaints
                .Where(c => c.categoryid == categoryid.Value)
                .ToList();

            ViewBag.StaffName =
                HttpContext.Session.GetString("staffname");

            ViewBag.TotalComplaints =
                complaints.Count;

            ViewBag.PendingComplaints =
                complaints.Count(c => c.status == "Pending");

            ViewBag.ResolvedComplaints =
                complaints.Count(c => c.status == "Resolved");

            List<StaffComplaintViewModel> complaintViewModels =
                new List<StaffComplaintViewModel>();

            foreach (var complaint in complaints)
            {
                var category = _context.Categories
                    .FirstOrDefault(c =>
                        c.categoryid == complaint.categoryid);

                var user = _context.Users
                    .FirstOrDefault(u =>
                        u.userid == complaint.userid);

                complaintViewModels.Add(
                    new StaffComplaintViewModel
                    {
                        complaintid = complaint.complaintid,
                        categoryid = complaint.categoryid,
                        categoryname = category?.categoryname,
                        userid = complaint.userid,
                        username = user?.name,
                        complaintname = complaint.complaintname,
                        description = complaint.description,
                        priority = complaint.priority,
                        status = complaint.status
                    }
                );
            }

            return View(complaintViewModels);
        }

        // ====================================================
        // PROFILE
        // ====================================================

        [HttpGet]
        public IActionResult Profile()
        {
            var userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var staff = _context.Users
                .FirstOrDefault(u => u.userid == userid && u.role == "Staff");

            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(User staff)
        {
            ModelState.Remove("password");
            ModelState.Remove("role");

            if (!ModelState.IsValid)
            {
                return View(staff);
            }

            var userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingStaff = _context.Users
                .FirstOrDefault(u => u.userid == userid && u.role == "Staff");

            if (existingStaff == null)
            {
                return NotFound();
            }

            existingStaff.name = staff.name;
            existingStaff.email = staff.email;
            existingStaff.country_code = staff.country_code;
            existingStaff.phone_no = staff.phone_no;

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        // ====================================================
        // COMPLAINTS
        // ====================================================

        public IActionResult Complaints()
        {
            int? userid = HttpContext.Session.GetInt32("userid");
            int? categoryid = HttpContext.Session.GetInt32("categoryid");

            if (userid == null || categoryid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var complaints = _context.Complaints
                .Where(c => c.categoryid == categoryid.Value)
                .ToList();

            List<StaffComplaintViewModel> complaintViewModels =
                new List<StaffComplaintViewModel>();

            foreach (var complaint in complaints)
            {
                var category = _context.Categories
                    .FirstOrDefault(c =>
                        c.categoryid == complaint.categoryid);

                var user = _context.Users
                    .FirstOrDefault(u =>
                        u.userid == complaint.userid);

                StaffComplaintViewModel viewModel =
                    new StaffComplaintViewModel
                    {
                        complaintid = complaint.complaintid,
                        categoryid = complaint.categoryid,
                        categoryname = category?.categoryname,
                        userid = complaint.userid,
                        username = user?.name,
                        complaintname = complaint.complaintname,
                        description = complaint.description,
                        priority = complaint.priority,
                        status = complaint.status
                    };

                complaintViewModels.Add(viewModel);
            }

            return View(complaintViewModels);
        }

        // ====================================================
        // DETAILS
        // ====================================================

        [HttpGet]
        public IActionResult Details(int id)
        {
            int? userid = HttpContext.Session.GetInt32("userid");
            int? categoryid = HttpContext.Session.GetInt32("categoryid");

            if (userid == null || categoryid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == id &&
                    c.categoryid == categoryid.Value);

            if (complaint == null)
            {
                return NotFound();
            }

            var category = _context.Categories
                .FirstOrDefault(c =>
                    c.categoryid == complaint.categoryid);

            var user = _context.Users
                .FirstOrDefault(u =>
                    u.userid == complaint.userid);

            StaffComplaintViewModel viewModel =
                new StaffComplaintViewModel
                {
                    complaintid = complaint.complaintid,
                    categoryid = complaint.categoryid,
                    categoryname = category?.categoryname,
                    userid = complaint.userid,
                    username = user?.name,
                    complaintname = complaint.complaintname,
                    description = complaint.description,
                    priority = complaint.priority,
                    status = complaint.status
                };

            return View(viewModel);
        }

        // ====================================================
        // UPDATE
        // ====================================================

        [HttpGet]
        public IActionResult Update(int id)
        {
            int? userid = HttpContext.Session.GetInt32("userid");
            int? categoryid = HttpContext.Session.GetInt32("categoryid");

            if (userid == null || categoryid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == id &&
                    c.categoryid == categoryid.Value);

            if (complaint == null)
            {
                return NotFound();
            }

            var category = _context.Categories
                .FirstOrDefault(c =>
                    c.categoryid == complaint.categoryid);

            var user = _context.Users
                .FirstOrDefault(u =>
                    u.userid == complaint.userid);

            StaffComplaintViewModel viewModel =
                new StaffComplaintViewModel
                {
                    complaintid = complaint.complaintid,
                    categoryid = complaint.categoryid,
                    categoryname = category?.categoryname,
                    userid = complaint.userid,
                    username = user?.name,
                    complaintname = complaint.complaintname,
                    description = complaint.description,
                    priority = complaint.priority,
                    status = complaint.status
                };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(StaffComplaintViewModel c1)
        {
            int? userid = HttpContext.Session.GetInt32("userid");
            int? categoryid = HttpContext.Session.GetInt32("categoryid");

            if (userid == null || categoryid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == c1.complaintid &&
                    c.categoryid == categoryid.Value);

            if (complaint == null)
            {
                return NotFound();
            }

            ModelState.Remove("username");
            ModelState.Remove("categoryname");

            if (!ModelState.IsValid)
            {
                return View(c1);
            }

            complaint.status = c1.status;

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        // ====================================================
        // LOGOUT
        // ====================================================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login",
                "Account"
            );
        }
    }
}
