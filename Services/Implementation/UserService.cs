using System;
using System.Text;
using System.Threading.Tasks;
using Backend_Dis_App.Database;
using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Backend_Dis_App.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly TaxAppContext _db;
        private readonly IEmailService _emailService;
        private readonly ISecurePassword _securePassword;
        private readonly IMemoryCache _memoryCache;

        public UserService(IEmailService emailService, ISecurePassword securePassword, IMemoryCache memoryCache)
        {
            _db = new TaxAppContext();
            _emailService = emailService;
            _securePassword = securePassword;
            _memoryCache = memoryCache;
        }

        public async Task CreateAccountAsync(User user)
        {
            var existingUser = await _db.User.FirstOrDefaultAsync(x => x.EmailAddress.Equals(user.EmailAddress));
            if (existingUser == null)
            {
                await _db.User.AddAsync(user);
                await _db.SaveChangesAsync();
            }
            else
                throw new Exception(string.Format("User {0} exists already.", user.EmailAddress));
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
            var user = await _db.User.FirstOrDefaultAsync(x => x.EmailAddress.Equals(loginModel.EmailAddress));
            if (user != null)
            {
                var x = Convert.FromBase64String(user.PasswordHash);
                var x2 = Encoding.ASCII.GetString(x);
                var computedHash = _securePassword.DecryptPassword(loginModel.Password, user.PasswordSalt);            

                if (user.PasswordHash.Equals(Convert.ToBase64String(computedHash)))
                    return true;
                return false;
            }
            else
                return false;
        }

        public bool LogOut()
        {
            try
            {
                _memoryCache.Remove("Login_Info");
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task ResetPasswordAsync(ResetPasswordModel resetPasswordModel)
        {
            var user = await _db.User.FirstOrDefaultAsync(x => x.EmailAddress.Equals(resetPasswordModel.EmailAddress));
            if (user != null)
            {
                var computedHash = _securePassword.DecryptPassword(resetPasswordModel.OldPassword, user.PasswordSalt);

                if (user.PasswordHash.Equals(Convert.ToBase64String(computedHash)))
                {
                    var (newPasswordHash, newSalt) = _securePassword.EncryptPassword(resetPasswordModel.NewPassword);
                    user.PasswordHash = Convert.ToBase64String(newPasswordHash);
                    user.PasswordSalt = Convert.ToBase64String(newSalt);
                    await _db.SaveChangesAsync();
                }
                else
                    throw new Exception(string.Format("Old password is wrong."));
            }
            else
                throw new Exception(string.Format("User {0} does not exists.", resetPasswordModel.EmailAddress));
        }
    }
}
