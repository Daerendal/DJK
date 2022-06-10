#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Merkado.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Merkado.DAL;
using Merkado.Utility;
using AspNetCore.ReCaptcha;

namespace Merkado.Areas.Identity.Pages.Account
{
    //[ValidateReCaptcha]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly MerkadoDbContext _merkadoDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            MerkadoDbContext merkadoDbContext,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _merkadoDbContext = merkadoDbContext;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Pole E-mail jest wymagane.")]
            [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Niepoprawny adres e-mail")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [StringLength(100, ErrorMessage = "{0} musi składać się minimum z {2} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("Password", ErrorMessage = "Hasła różnią się od siebie.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [Display(Name = "Imię")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [Display(Name = "Nazwisko")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [Display(Name = "Ulica")]
            public string Street { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [Display(Name = "Miasto")]
            public string City { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [Display(Name = "Kod pocztowy")]
            [RegularExpression(@"\d{2}-\d{3}", ErrorMessage = "Niepoprawny kod pocztowy")]
            public string PostalCode { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    City = Input.City,
                    PostalCode = Input.PostalCode,
                    Street = Input.Street
                };

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(Roles.User))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.User));
                    }

                    await _userManager.AddToRoleAsync(user, Roles.User);
                    _logger.LogInformation("Konto zostało pomyślnie utworzone.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Potwierdź swój e-mail.",
                        $"Proszę potwierdzić konto poprzez <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>kliknięcie tutaj</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
        
        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
