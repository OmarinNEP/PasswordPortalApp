using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using PasswordPortalApp.Services;

namespace PasswordPortalApp.Pages
{
    public class RequestResetModel : PageModel
    {
        private readonly EmailService _emailService;

        public RequestResetModel(EmailService emailService)
        {
            _emailService = emailService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Please enter your email or username")]
        public string UserIdentifier { get; set; }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Nothing needed here for now
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // TODO: generate token and store in DB or memory
                var token = Guid.NewGuid().ToString("N");

                // TODO: associate token with user in DB (UserIdentifier)
                
                // Send email
                await _emailService.SendPasswordResetEmail(UserIdentifier, token);

                SuccessMessage = "A password reset email has been sent if the account exists.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to send reset email: {ex.Message}";
            }

            return Page();
        }
    }
}
