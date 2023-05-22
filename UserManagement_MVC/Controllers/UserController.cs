using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement_MVC.Models;
using UserManagement_MVC.Models.Responses;
using UserManagement_MVC.Services;

namespace UserManagement_MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserModel registerUserModel)
        {
            RegisterUserResponse registerUserResponse = await _userService.Register(registerUserModel);

            if(registerUserResponse.IsSuccess) 
            {
                return Redirect("~/User/Login");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserModel loginUserModel)
        {
            LoginUserResponse loginUserResponse= await _userService.Login(loginUserModel);

            if (loginUserResponse.IsSuccess)
            {
                //HttpContext.Session.SetString("JWToken", loginUserResponse.jwtToken);
                _httpContextAccessor.HttpContext.Session.SetString("JWToken", loginUserResponse.jwtToken);

                //_httpContextAccessor.HttpContext.User.AddIdentities.Add(loginUserResponse.jwtToken);

                //_httpContextAccessor.HttpContext.User.AddIdentity.Add(loginUserResponse.jwtToken);

                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Username or Password");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userList = await _userService.Index();

            return View(userList);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Update(Guid Id)
        {
           UpdateUserModel updateUserModel = await _userService.Update(Id);

            if(updateUserModel != null)
            {
                return await Task.Run(() => View("Update", updateUserModel));
            }
            return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserModel updateUserModel)
        {
            await _userService.Update(updateUserModel);
            return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid Id)
        {
            await _userService.Delete(Id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult GetCurrentUser()
        {
            var res = _httpContextAccessor.HttpContext.User;

            return Ok(res);
        }
    }
}

