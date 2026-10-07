using System.Security.Claims;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("hangfire-login")]
    [AllowAnonymous]
    public class HangfireAuthController: ControllerBase
    {
        private readonly IIdentityService _identityService;

        public HangfireAuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        [HttpGet]
        public IActionResult ShowLoginForm()
        {
            const string html = """
                <html><body style="font-family: sans-serif; max-width: 320px; margin: 80px auto;">
                    <h3>Hangfire Dashboard Login</h3>
                    <form method="post">
                        <input name="email" type="email" placeholder="Email" style="width:100%; margin-bottom:8px; padding:6px;" required />
                        <input name="password" type="password" placeholder="Password" style="width:100%; margin-bottom:8px; padding:6px;" required />
                        <button type="submit" style="width:100%; padding:8px;">Login</button>
                    </form>
                </body></html>
                """;

            return Content(html, "text/html");
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
        {
            var result = await _identityService.LoginAsync(email, password);

            if (!result.Succeeded || !result.Roles.Contains(Roles.Admin))
            {
                return Content(
                    "<html><body>Invalid credentials or insufficient permissions. <a href=\"/hangfire-login\">Try again</a></body></html>",
                    "text/html");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, result.UserId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, Roles.Admin)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("HangfireAuth", principal);

            return Redirect("/hangfire");
        }
    }
}
