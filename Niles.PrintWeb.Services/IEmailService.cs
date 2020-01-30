using System.Threading.Tasks;

namespace Niles.PrintWeb.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        string CreateVerifyMailMessageContent(string subject, string message, string userLink);
    }
}