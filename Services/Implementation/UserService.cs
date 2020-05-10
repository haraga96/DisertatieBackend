using Backend_Dis_App.Database;
using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;

namespace Backend_Dis_App.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly TaxAppContext _db;
        public UserService()
        {
            _db = new TaxAppContext();
        }

        public void CreateAccount(User user)
        {
            _db.User.Add(user);
            _db.SaveChanges();
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
