using System.Threading.Tasks;

namespace Niles.PrintWeb.Api.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        string CreateVerifyMailMessageContent(string subject, string message, string userLink);
    }
}