using System.Security.Claims;
using complaint_mangement_system.Models;
using complaint_mangement_system.Repositories;
using complaint_mangement_system.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        public async Task<IActionResult> Login(LoginViewModel l1)
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
                await SignInUser(user);

                return RedirectToAction(
                    "Dashboard",
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
                    await SignInUser(
                        user,
                        staffRequest.categoryid
                    );

                    return RedirectToAction(
                        "Dashboard",
                        "Staff"
                    );
                }
            }

            if (user.role == "Admin")
            {
                await SignInUser(user);

                return RedirectToAction(
                    "Dashboard",
                    "Admin"
                );
            }

            ModelState.AddModelError(
                "",
                "Invalid user role."
            );

            return View(l1);
        }

        private async Task SignInUser(
            User user,
            int? categoryid = null
        )
        {
            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.userid.ToString()
                ),

                new Claim(
                    ClaimTypes.Name,
                    user.name
                ),

                new Claim(
                    ClaimTypes.Role,
                    user.role
                )
            };

            if (categoryid.HasValue)
            {
                claims.Add(
                    new Claim(
                        "categoryid",
                        categoryid.Value.ToString()
                    )
                );
            }

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );
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
        public async Task<IActionResult> RegisterAsUser(
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

            await SignInUser(user);

            return RedirectToAction(
                "Dashboard",
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

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction(
                "Login",
                "Account"
            );
        }
    }
}