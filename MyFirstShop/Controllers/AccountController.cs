using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using MyFirstShop.Data.Repositories;
using MyFirstShop.Models;
using System.Security.Claims;

namespace MyFirstShop.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            if(!ModelState.IsValid)
            {
                return View(register);
            }

            //if(_userRepository.isExistByUserEmail(register.Email.ToLower()))
            //{
            //    ModelState.AddModelError("Email", "ایمیل وارد شده قبلا ثبت نام کرده است.");
            //    return View(register);
            //}

            //if (_userRepository.isExistByUserName(register.UserName))
            //{
            //    ModelState.AddModelError("UserName", "نام کاربری وارد شده قبلا ثبت نام کرده است.");
            //    return View(register);
            //}

            Users user = new Users()
            {
                UserName = register.UserName,
                Email = register.Email.ToLower(),
                RegisterDate = DateTime.Now,
                Password = register.Password,
                IsAdmin = false
            };
            _userRepository.addUser(user);

            return View("SuccessRegister",register);
        }
        #endregion

        public IActionResult VerifyEmail(string email)
        {
			if (_userRepository.isExistByUserEmail(email.ToLower()))
            {
                return Json($"ایمیل وارد شده تکراری می باشد.");
            }
            return Json(true);

		}

        public IActionResult VerifyUserName(string userName)
        {
			if (_userRepository.isExistByUserName(userName))
            {
                return Json($"نام کاربری وارد شده تکراری می باشد.");
            }
            return Json(true);

		}

		#region Login
		public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var user = _userRepository.GetUserByLogin(login.Email.ToLower(),login.Password);    

            if(user == null)
            {
                ModelState.AddModelError("Emali", "اطلاعات وارد شده صحیح نمی باشد");
                return View(login);
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.UserName)
            };

            var identify = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identify);

            var properties = new AuthenticationProperties
            {
                IsPersistent = login.RememberMe
            };

            HttpContext.SignInAsync(principal, properties);

            return Redirect("/");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Account/Login");
        }

        #endregion
    }
}
