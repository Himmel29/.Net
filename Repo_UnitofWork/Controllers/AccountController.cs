using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Repo_UnitofWork.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            
            return View();
        }
        // Đăng nhập qua Google
        [HttpGet]
        public async Task<IActionResult> LoginWithGoogle(string returnUrl = "/")
        {
            // Nếu người dùng đã đăng nhập, đăng xuất người dùng hiện tại
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse"), // Sau khi đăng nhập thành công, redirect về GoogleResponse
                Items = { { "returnUrl", returnUrl } }
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Xử lý phản hồi từ Google
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (result?.Principal != null)
            {
                // Lấy thông tin user từ Google
                var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
                var email = claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
                var name = claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;

                // Lưu thông tin user vào session hoặc database
                TempData["Message"] = $"Logged in as {name} ({email})";

                return RedirectToAction("Index", "Home");
            }

            // Nếu không thành công, quay lại trang đăng nhập
            TempData["Error"] = "Google login failed.";
            return RedirectToAction("Login");
        }

        // Đăng xuất
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
