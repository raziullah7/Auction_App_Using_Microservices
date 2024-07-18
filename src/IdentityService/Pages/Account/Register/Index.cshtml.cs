using System.Security.Claims;
using IdentityModel;
using IdentityService.Models;
using IdentityService.Pages.Account.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        // dependency injection
        private readonly UserManager<ApplicationUser> _userManager;

        // binding property for view model
        [BindProperty]
        public RegisterViewModel Input { get; set; }

        // binding property for result
        [BindProperty]
        public bool RegisterSuccess { get; set; }

        public Index(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET method of Register Page
        public IActionResult OnGet(string returnUrl)
        {
            Input = new RegisterViewModel
            {
                ReturnUrl = returnUrl,
            };

            return Page();
        }

        // POST method for Register Page
        public async Task<IActionResult> OnPost()
        {
            // if Cancel Button is pressed, redirect to home page
            if (Input.Button != "register") return Redirect("~/");

            // if register button is clicked and all fields are filled
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimsAsync(user, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, Input.FullName)
                    });

                    RegisterSuccess = true;
                }
            }

            return Page();
        }
    }
}