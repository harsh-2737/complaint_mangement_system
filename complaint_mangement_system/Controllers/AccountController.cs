using complaint_mangement_system.Repositories;
using complaint_mangement_system.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace complaint_mangement_system.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel l1)
        {
            if (!ModelState.IsValid)
            {
                return View(l1);
            }

            var user = _accountRepository.GetUserByEmail(l1.email);

            if (user == null)
            {
                TempData["RegisterMessage"] =
                    "Email not found. Please register first.";

                return RedirectToAction("RegisterAsUser");
            }

            if (user.password != l1.password)
            {
                ModelState.AddModelError(
                    "",
                    "Incorrect password."
                );

                return View(l1);
            }

            if (user.role == "User")
            {
                HttpContext.Session.SetInt32(
                    "userid",
                    user.userid
                );

                return RedirectToAction(
                    "DashBoard",
                    "User"
                );
            }

            if (user.role == "Staff")
            {
                var staffRequest =
                    _accountRepository.GetStaffRequest(user.userid);

                if (staffRequest == null)
                {
                    ModelState.AddModelError(
                        "",
                        "Staff registration request not found."
                    );

                    return View(l1);
                }

                if (staffRequest.status == "Pending")
                {
                    return View("StaffRequestPending");
                }

                if (staffRequest.status == "Rejected")
                {
                    return View("StaffRequestRejected");
                }

                if (staffRequest.status == "Approved")
                {
                    HttpContext.Session.SetInt32(
                        "userid",
                        user.userid
                    );

                    HttpContext.Session.SetInt32(
                        "categoryid",
                        staffRequest.categoryid
                    );

                    return RedirectToAction(
                        "DashBoard",
                        "Staff"
                    );
                }
            }

            if (user.role == "Admin")
            {
                HttpContext.Session.SetInt32(
                    "userid",
                    user.userid
                );

                return RedirectToAction(
                    "DashBoard",
                    "Admin"
                );
            }

            ModelState.AddModelError(
                "",
                "Invalid user role."
            );

            return View(l1);
        }

        [HttpGet]
        public IActionResult RegisterAsUser()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterAsStaff()
        {
            var categories =
                _accountRepository.GetCategories();

            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        public IActionResult RegisterAsUser(
            UserRegisterViewModel r1)
        {
            r1.role = "User";

            ModelState.Remove("role");

            if (!ModelState.IsValid)
            {
                return View(r1);
            }

            var existingUser =
                _accountRepository.GetUserByEmail(r1.email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "email",
                    "Email already exists."
                );

                return View(r1);
            }

            var user =
                _accountRepository.RegisterAsUser(r1);

            HttpContext.Session.SetInt32(
                "userid",
                user.userid
            );

            return RedirectToAction(
                "DashBoard",
                "User"
            );
        }

        [HttpPost]
        public IActionResult RegisterAsStaff(
            StaffRegisterViewModel r1)
        {
            r1.role = "Staff";
            r1.status = StaffStatus.Pending;

            ModelState.Remove("role");
            ModelState.Remove("status");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories =
                    _accountRepository.GetCategories();

                return View(r1);
            }

            var existingUser =
                _accountRepository.GetUserByEmail(r1.email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "email",
                    "Email already exists."
                );

                ViewBag.Categories =
                    _accountRepository.GetCategories();

                return View(r1);
            }

            var user =
                _accountRepository.RegisterAsStaff(r1);

            _accountRepository.CreateStaffRequest(
                user.userid,
                r1.categoryid
            );

            return View("StaffRequestSent");
        }

        public IActionResult StaffRequestSent()
        {
            return View();
        }
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