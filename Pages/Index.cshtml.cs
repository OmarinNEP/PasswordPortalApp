using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace PasswordPortalApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Username or email is required")]
        public string UserIdentifier { get; set; } = string.Empty;

        public bool ShowConfirmation { get; set; } = false;

        public void OnGet()
        {
            // Initial page load
        }

        public void OnPost()
        {
            // IMPORTANT:
            // We intentionally do NOT confirm whether the user exists
            // This prevents account enumeration attacks

            // Later this is where we will:
            // 1. Lookup AD user
            // 2. Generate secure token
            // 3. Email reset link

            ShowConfirmation = true;
        }
    }
}
