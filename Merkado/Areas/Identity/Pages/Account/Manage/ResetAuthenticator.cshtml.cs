#nullable disable

using System;
using System.Threading.Tasks;
using DJK.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DJK.Areas.Identity.Pages.Account.Manage
{
    public class ResetAuthenticatorModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<ResetAuthenticatorModel> _logger;

        public ResetAuthenticatorModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<ResetAuthenticatorModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można znaleźć użytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można znaleźć użytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            _logger.LogInformation("Użytkownik o ID '{UserId}' zresetował kod 2FA.", user.Id);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Twój klucz aplikacji uwierzytelniającej został zresetowany, musisz skonfigurować aplikację uwierzytelniającą przy użyciu nowego klucza.";

            return RedirectToPage("./EnableAuthenticator");
        }
    }
}
