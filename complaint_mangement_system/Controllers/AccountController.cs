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

                return RedirectToAction("Register");
            }

           
            if (user.password != l1.password)
            {
                ModelState.AddModelError(
                    "",
                    "Incorrect password."
                );

                return View(l1);
            }

            
            HttpContext.Session.SetInt32(
                "userid",
                user.userid
            );

          
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


       

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }



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
                _accountRepository.Register(r1);

          
            HttpContext.Session.SetInt32(
                "userid",
                user.userid
            );

            
            return RedirectToAction(
                "DashBoard",
                "User"
            );
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }
    }
}