
 using System.Threading.Tasks;
    namespace ToDoApp.Services
    {
        public interface IEmailService
        {
            Task SendEmailAsync(string recipient, string subject, string body);
        }
    }


