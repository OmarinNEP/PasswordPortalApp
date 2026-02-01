using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace PasswordPortalApp.Pages
{
    public class ResetPasswordModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Token { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string NewPassword { get; set; } = string.Empty;

        [BindProperty]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public bool IsTokenValid { get; set; } = true;
        public bool ShowConfirmation { get; set; } = false;

        public void OnGet()
        {
            // Later:
            // IsTokenValid = TokenService.Validate(Token);

            if (string.IsNullOrWhiteSpace(Token))
            {
                IsTokenValid = false;
            }
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
                return;

            // Later:
            // 1. Validate token
            // 2. Identify AD user
            // 3. Reset password in AD
            // 4. Invalidate token

            ShowConfirmation = true;
        }
    }
}
