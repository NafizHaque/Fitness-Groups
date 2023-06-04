using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroups.Data;
using RunGroups.DTOs.AccountDTOs;
using RunGroups.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RunGroups.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (!ModelState.IsValid) { return View(login); }
            AppUser user = await _userManager.FindByEmailAsync(login.Email);

            if (user != null)
            {
                Boolean passwordCheck = await _userManager.CheckPasswordAsync(user, login.Password);
                if (passwordCheck)
                {
                    SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("index", "Home");
                    }
                }
            }
            TempData["Error"] = "Wrong Email or Password.";
            return View(login);
        }

        public IActionResult Register()
        {
            RegisterDto response = new RegisterDto();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) { return View(registerDto); }

            AppUser user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
            {
                TempData["Error"] = "Email Account Already Exists";
                return View(registerDto);
            }

            AppUser newUser = new AppUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.Username,
            };
            IdentityResult newUserResponse = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            return RedirectToAction("index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }




    }
}
