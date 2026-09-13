using Microsoft.AspNetCore.Mvc;
using complaint_mangement_system.Models;
using complaint_mangement_system.Data;
using complaint_mangement_system.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace complaint_mangement_system.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int? userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("Dashboard");
        }


        // ----------------------------------------------------
        // DASHBOARD
        // ----------------------------------------------------
        public IActionResult Dashboard()
        {
            int? userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var complaints = _context.Complaints
                .Where(c => c.userid == userid.Value)
                .ToList();

            ViewBag.UserName =
                HttpContext.Session.GetString("username");

            ViewBag.TotalComplaints =
                complaints.Count;

            ViewBag.PendingComplaints =
                complaints.Count(c => c.status == "Pending");

            ViewBag.ResolvedComplaints =
                complaints.Count(c => c.status == "Resolved");

            List<UserComplaintViewModel> complaintViewModels = new List<UserComplaintViewModel>();

            foreach (var complaint in complaints)
            {
                var category = _context.Categories
                    .FirstOrDefault(c =>
                        c.categoryid == complaint.categoryid);

                complaintViewModels.Add(new UserComplaintViewModel
                {
                    complaintid = complaint.complaintid,

                    categoryid = complaint.categoryid,
                    categoryname = category?.categoryname,

                    userid = complaint.userid,

                    complaintname = complaint.complaintname,
                    description = complaint.description,
                    priority = complaint.priority,
                    status = complaint.status
                });
            }

            return View(complaintViewModels);
        }


        // ----------------------------------------------------
        // CREATE - GET
        // ----------------------------------------------------
        [HttpGet]
        public IActionResult Create()
        {
            int? userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            LoadCategories();

            return View();
        }


        // ----------------------------------------------------
        // CREATE - POST
        // ----------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserComplaintViewModel c1)
        {
            int? userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // These values are not entered by the user.
            // They are handled by the controller.
            ModelState.Remove("status");
            ModelState.Remove("categoryname");

            if (!ModelState.IsValid)
            {
                LoadCategories(c1.categoryid);

                return View(c1);
            }

            // Check whether selected category exists
            var category = _context.Categories
                .FirstOrDefault(c => c.categoryid == c1.categoryid);

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
                userid = userid.Value,
                complaintname = c1.complaintname,
                description = c1.description,
                priority = c1.priority,

                // User does not select status
                status = "Pending"
            };

            _context.Complaints.Add(complaint);

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }


        // ----------------------------------------------------
        // PROFILE
        // ----------------------------------------------------
        [HttpGet]
        public IActionResult Profile()
        {
            var userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users
                .FirstOrDefault(u => u.userid == userid && u.role == "User");

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

            var userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingUser = _context.Users
                .FirstOrDefault(u => u.userid == userid && u.role == "User");

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

        // ----------------------------------------------------
        // COMPLAINTS
        // ----------------------------------------------------
        public IActionResult Complaints()
        {
            int? userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var complaints = _context.Complaints
                .Where(c => c.userid == userid.Value)
                .ToList();

            List<UserComplaintViewModel> complaintViewModels = new List<UserComplaintViewModel>();

            foreach (var complaint in complaints)
            {
                var category = _context.Categories
                    .FirstOrDefault(c =>
                        c.categoryid == complaint.categoryid);

                UserComplaintViewModel viewModel = new UserComplaintViewModel
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


        // ----------------------------------------------------
        // UPDATE - GET
        // ----------------------------------------------------
        [HttpGet]
        public IActionResult Update(int id)
        {
            int? userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == id &&
                    c.userid == userid.Value);

            if (complaint == null)
            {
                return NotFound();
            }

            var category = _context.Categories
                .FirstOrDefault(c =>
                    c.categoryid == complaint.categoryid);

            UserComplaintViewModel viewModel = new UserComplaintViewModel
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


        // ----------------------------------------------------
        // UPDATE - POST
        // ----------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(UserComplaintViewModel c1)
        {
            int? userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == c1.complaintid &&
                    c.userid == userid.Value);

            if (complaint == null)
            {
                return NotFound();
            }

            // These are display/server-side values
            ModelState.Remove("status");
            ModelState.Remove("categoryname");

            if (!ModelState.IsValid)
            {
                LoadCategories(c1.categoryid);

                return View(c1);
            }

            // Check category
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

            // Update only fields the user is allowed to change
            complaint.categoryid = c1.categoryid;
            complaint.complaintname = c1.complaintname;
            complaint.description = c1.description;
            complaint.priority = c1.priority;

            // Do NOT allow user to change status
            // Status is controlled by Admin/Staff

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }


        // ----------------------------------------------------
        // CONFIRM DELETE
        // ----------------------------------------------------
        [HttpGet]
        public IActionResult ConfirmDelete(int id)
        {
            int? userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == id &&
                    c.userid == userid.Value);

            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }


        // ----------------------------------------------------
        // DELETE
        // ----------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            int? userid = HttpContext.Session.GetInt32("userid");

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var complaint = _context.Complaints
                .FirstOrDefault(c =>
                    c.complaintid == id &&
                    c.userid == userid.Value);

            if (complaint == null)
            {
                return NotFound();
            }

            _context.Complaints.Remove(complaint);

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }


        // ----------------------------------------------------
        // LOGOUT
        // ----------------------------------------------------
       


        // ----------------------------------------------------
        // LOAD CATEGORIES
        // ----------------------------------------------------
        private void LoadCategories(int? selectedCategoryId = null)
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