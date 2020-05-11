using System;
using System.Threading.Tasks;

namespace Backend_Dis_App.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject);
    }
}
