using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebUygulamaProje1.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            //sizler buraya email gönderme işlemlerinizi yapabilirsiniz
            return Task.CompletedTask;
        }
    }
}
