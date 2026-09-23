using System.Security.Claims;
using complaint_mangement_system.Data;
using complaint_mangement_system.Models;
using complaint_mangement_system.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace complaint_mangement_system.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StaffController(ApplicationDbContext context)
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
            int categoryid = GetCategoryId();

            var complaints = _context.Complaints
                .Where(c => c.categoryid == categoryid)
                .ToList();

            var staff = _context.Users
                .FirstOrDefault(u => u.userid == userid);

            ViewBag.StaffName = staff?.name;

            ViewBag.TotalComplaints =
                complaints.Count;

            ViewBag.PendingComplaints =
                complaints.Count(c => c.status == "Pending");

            ViewBag.InProgressComplaints =
                complaints.Count(c => c.status == "In Progress");

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

                        status = complaint.status
                    }
                );
            }

            return View(complaintViewModels);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            int userid = GetUserId();

            var staff = _context.Users
                .FirstOrDefault(u =>
                    u.userid == userid &&
                    u.role == "Staff");

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

            int userid = GetUserId();

            var existingStaff = _context.Users
                .FirstOrDefault(u =>
                    u.userid == userid &&
                    u.role == "Staff");

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

        public IActionResult Complaints()
        {
            int categoryid = GetCategoryId();

            var complaints = _context.Complaints
                .Where(c => c.categoryid == categoryid)
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
                        status = complaint.status
                    };

                complaintViewModels.Add(viewModel);
            }

            return View(complaintViewModels);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            int categoryid = GetCategoryId();

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == id &&
                    c.categoryid == categoryid);

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
                    categoryname = category?.categoryname,
                    username = user?.name,
                    country_code = user?.country_code,
                    phone_no = user?.phone_no,
                    complaintname = complaint.complaintname,
                    description = complaint.description
                };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            int categoryid = GetCategoryId();

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == id &&
                    c.categoryid == categoryid);

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
                    status = complaint.status
                };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(StaffComplaintViewModel c1)
        {
            int categoryid = GetCategoryId();

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == c1.complaintid &&
                    c.categoryid == categoryid);

            if (complaint == null)
            {
                return NotFound();
            }

            ModelState.Remove("categoryid");
            ModelState.Remove("userid");
            ModelState.Remove("username");
            ModelState.Remove("country_code");
            ModelState.Remove("phone_no");
            ModelState.Remove("categoryname");
            ModelState.Remove("complaintname");
            ModelState.Remove("description");

            if (!ModelState.IsValid)
            {
                return View(c1);
            }

            complaint.status = c1.status;

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult StaffMembers()
        {
            int categoryid = GetCategoryId();
            int currentStaffId = GetUserId();

            var staffIds = _context.StaffRequests
                .Where(r =>
                    r.categoryid == categoryid &&
                    r.status == "Approved" &&
                    r.userid != currentStaffId)
                .Select(r => r.userid)
                .ToList();

            var staffMembers = _context.Users
                .Where(u =>
                    staffIds.Contains(u.userid) &&
                    u.role == "Staff")
                .Select(u => new StaffMemberViewModel
                {
                    userid = u.userid,
                    name = u.name,
                    email = u.email,
                    country_code = u.country_code,
                    phone_no = u.phone_no
                })
                .ToList();

            return View(staffMembers);
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

        private int GetCategoryId()
        {
            return int.Parse(
                User.FindFirstValue(
                    "categoryid"
                )
            );
        }
    }
}