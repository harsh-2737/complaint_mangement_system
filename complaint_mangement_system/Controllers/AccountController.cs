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

        // =========================
        // LOGIN GET
        // =========================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        // =========================
        // LOGIN POST
        // =========================

        [HttpPost]
        public IActionResult Login(LoginViewModel l1)
        {
            if (!ModelState.IsValid)
            {
                return View(l1);
            }

            // Get user using repository
            var user = _accountRepository.GetUserByEmail(l1.email);

            // Email does not exist
            if (user == null)
            {
                TempData["RegisterMessage"] =
                    "Email not found. Please register first.";

                return RedirectToAction("Register");
            }

            // Check password
            if (user.password != l1.password)
            {
                ModelState.AddModelError(
                    "",
                    "Incorrect password."
                );

                return View(l1);
            }

            // Store userid in session
            HttpContext.Session.SetInt32(
                "userid",
                user.userid
            );

            // Redirect according to role
            if (user.role == "User")
            {
                return RedirectToAction(
                    "DashBoard",
                    "User"
                );
            }

            if (user.role == "Staff")
            {
                return RedirectToAction(
                    "DashBoard",
                    "Staff"
                );
            }

            if (user.role == "Admin")
            {
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


        // =========================
        // REGISTER GET
        // =========================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        // =========================
        // REGISTER POST
        // =========================

        [HttpPost]
        public IActionResult Register(RegisterViewModel r1)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine(
                            $"{item.Key}: {error.ErrorMessage}"
                        );
                    }
                }

                return View(r1);
            }

            // Check whether email already exists
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

            // Register user using repository
            var user =
                _accountRepository.Register(r1);

            // Store userid in session
            HttpContext.Session.SetInt32(
                "userid",
                user.userid
            );

            // Go to user dashboard
            return RedirectToAction(
                "DashBoard",
                "User"
            );
        }
    }
}