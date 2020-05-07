using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;

namespace Backend_Dis_App.Services.Implementation
{
    public class UserService : IUserService
    {
        public UserService()
        {
        }

        public void CreateAccount(CreateAccountModel newAccountModel)
        {
            
        }

        public void ForgotPassword(long id)
        {
            
        }

        public bool Login(LoginModel loginModel)
        {
            if (!string.IsNullOrWhiteSpace(loginModel.EmailAddress) || !string.IsNullOrWhiteSpace(loginModel.Password))
                return true;
            return false;
        }

        public bool LogOut()
        {
            return true;
        }
    }
}
