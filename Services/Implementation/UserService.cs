using System;
using System.Linq;
using System.Threading.Tasks;
using Backend_Dis_App.Database;
using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;

namespace Backend_Dis_App.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly TaxAppContext _db;
        private readonly IEmailService _emailService;

        public UserService( IEmailService emailService)
        {
            _db = new TaxAppContext();
            _emailService = emailService;
        }

        public void CreateAccount(User user)
        {
            _db.User.Add(user);
            _db.SaveChanges();
        }

        public void ForgotPassword(string emailAddress)
        {
            var user = _db.User.FirstOrDefault(x => x.EmailAddress.Equals(emailAddress));
            if (user != null)
            {
                _emailService.SendEmail(emailAddress, "Forgot password");
            }
            else
            {
                throw new Exception(string.Format("User {0} does not exists.", emailAddress));
            }
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

        public void ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var user = _db.User.FirstOrDefault(x => x.EmailAddress.Equals(resetPasswordModel.EmailAddress));
            if (user != null)
            {
                if (user.PasswordHash.Equals(resetPasswordModel.OldPassword))
                {
                    user.PasswordHash = resetPasswordModel.NewPassword;
                    _db.SaveChanges();
                }
                else
                    throw new Exception(string.Format("Old password is wrong."));
            }
            else
                throw new Exception(string.Format("User {0} does not exists.",resetPasswordModel.EmailAddress));
        }
    }
}
