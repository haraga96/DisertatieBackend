using Backend_Dis_App.Models;

namespace Backend_Dis_App.Services.Interfaces
{
    public interface IUserService
    {
        bool Login(LoginModel loginModel);
        void CreateAccount(CreateAccountModel newAccountModel);
        bool LogOut();
        void ForgotPassword(long id);
    }
}
