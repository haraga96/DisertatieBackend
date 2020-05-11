using System;
using System.Linq;
using System.Threading.Tasks;
using Backend_Dis_App.Database;
using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task CreateAccountAsync(User user)
        {
            await _db.User.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task ForgotPasswordAsync(string emailAddress)
        {
            var user = await _db.User.FirstOrDefaultAsync(x => x.EmailAddress.Equals(emailAddress));
            if (user != null)
            {
                await _emailService.SendEmailAsync(emailAddress, "Forgot password");
            }
            else
            {
                throw new Exception(string.Format("User {0} does not exists.", emailAddress));
            }
        }

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            var user = await _db.User.FirstOrDefaultAsync(x => x.EmailAddress.Equals(loginModel.EmailAddress) && x.PasswordHash.Equals(loginModel.Password));
            if (user != null)
                return true;
            return false;
        }

        public async Task<bool> LogOutAsync()
        {
            return true;
        }

        public async Task ResetPasswordAsync(ResetPasswordModel resetPasswordModel)
        {
            var user = await _db.User.FirstOrDefaultAsync(x => x.EmailAddress.Equals(resetPasswordModel.EmailAddress));
            if (user != null)
            {
                if (user.PasswordHash.Equals(resetPasswordModel.OldPassword))
                {
                    user.PasswordHash = resetPasswordModel.NewPassword;
                    await _db.SaveChangesAsync();
                }
                else
                    throw new Exception(string.Format("Old password is wrong."));
            }
            else
                throw new Exception(string.Format("User {0} does not exists.",resetPasswordModel.EmailAddress));
        }
    }
}
