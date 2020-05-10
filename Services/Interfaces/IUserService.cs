using Backend_Dis_App.Models;

namespace Backend_Dis_App.Services.Interfaces
{
    public interface IUserService
    {
        bool Login(LoginModel loginModel);
        void CreateAccount(User user);
        bool LogOut();
        void ForgotPassword(long id);
    }
}
