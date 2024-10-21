using Microsoft.AspNetCore.Identity.UI.Services;

namespace ATeam_MVC_WebApp.Services
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Log the email or do nothing
            return Task.CompletedTask;
        }
    }
}