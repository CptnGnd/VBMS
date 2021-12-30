using VBMS.Application.Requests.Mail;
using System.Threading.Tasks;

namespace VBMS.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}