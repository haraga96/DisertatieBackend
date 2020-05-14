using System.Threading.Tasks;
using Backend_Dis_App.Database;
using Backend_Dis_App.Models;

namespace Backend_Dis_App.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> LoginAsync(LoginModel loginModel);
        Task CreateAccountAsync(User user);
        bool LogOut();
        Task ForgotPasswordAsync(string emailAddress);
        Task ResetPasswordAsync(ResetPasswordModel resetPasswordModel);
    }
}
