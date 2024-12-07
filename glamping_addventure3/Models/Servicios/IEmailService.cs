using System.Threading.Tasks;

namespace Glamping_Addventure3.Services
{
    public interface IEmailService
    {
        Task EnviarCorreoRecuperacion(string email, string subject, string message);
    }
}
