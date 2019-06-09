namespace SmartSchedule.Application.Interfaces
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string message);
    }
}
