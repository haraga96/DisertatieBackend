using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend_Dis_App.BaseClasses;
using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;
using Backend_Dis_App.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Backend_Dis_App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IEmailValidator _emailValidator;
        private readonly IPasswordValidator _passwordValidator;
        private readonly ISecurePassword _securePassword;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private string ErrorMessage { get; set; }

        public UsersController(IConfiguration configuration,
            IUserService userService,
            IEmailValidator emailValidator,
            IPasswordValidator passwordValidator,
            ISecurePassword securePassword,
            IMemoryCache memoryCache)
        {
            _userService = userService;
            _emailValidator = emailValidator;
            _passwordValidator = passwordValidator;
            _securePassword = securePassword;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("checkstatus")]
        public IActionResult CheckStatus()
        {
            var tokenValue = _memoryCache.Get("Login_Info")?.ToString();
            if (!string.IsNullOrWhiteSpace(tokenValue))
                return Ok(tokenValue);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginIntoAccount([FromBody] LoginModel user)
        {
            if (string.IsNullOrWhiteSpace(user.EmailAddress) || string.IsNullOrWhiteSpace(user.Password))
            {
                ErrorMessage = "Please fill all required fields.";
                return BadRequest(ErrorMessage);
            }
            var isOk = await _userService.LoginAsync(user);
            if (isOk)
            {
                var tokenKey = Encoding.ASCII.GetBytes(_configuration["Token:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                                {
                                    new Claim(ClaimTypes.Name, user.EmailAddress),
                                    new Claim(ClaimTypes.Role, "Administrator")
                                }),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var writenToken = tokenHandler.WriteToken(token);
                var options = new MemoryCacheEntryOptions().SetSize(1).SetAbsoluteExpiration(DateTime.Now.AddDays(7));
                _memoryCache.Set("Login_Info", writenToken, options);
                return Ok(writenToken);
            }
            else
                return Unauthorized("Invalid credentials. Try again");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] User user)
        {
            if (!_emailValidator.CheckRule(user.EmailAddress))
            {
                ErrorMessage = "Email invalid format.";
                return BadRequest(ErrorMessage);
            }
            try
            {
                await _userService.ForgotPasswordAsync(user.EmailAddress);
                return Ok("Email has been sent");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("logout")]
        public IActionResult LogOut()
        {
            var isLoggedOut = _userService.LogOut();
            if (isLoggedOut)
                return Ok();
            else
                return BadRequest("Something went wrong");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel user)
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(user.EmailAddress) ||
                string.IsNullOrWhiteSpace(user.OldPassword) ||
                string.IsNullOrWhiteSpace(user.NewPassword) ||
                string.IsNullOrWhiteSpace(user.ConfirmNewPassword))
            {
                ErrorMessage = "Please fill all required fields.";
                return BadRequest(ErrorMessage);
            }
            if (!_emailValidator.CheckRule(user.EmailAddress))
            {
                ErrorMessage = "Email invalid format.";
                return BadRequest(ErrorMessage);
            }
            if (!_passwordValidator.CheckRule(user.NewPassword))
            {
                ErrorMessage += "*Password invalid format." +
                    " It must contains 8 characters, at least one letter and one digit.";
                isValid = false;
            }
            if (!user.NewPassword.Equals(user.ConfirmNewPassword))
            {
                ErrorMessage += "*Password mismatch.";
                isValid = false;
            }
            try
            {
                if (isValid)
                {
                    await _userService.ResetPasswordAsync(user);
                    return Ok("Password has been changed.");
                }
                else
                    return BadRequest(ErrorMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountModel user)
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.EmailAddress) ||
                string.IsNullOrWhiteSpace(user.Password) ||
                string.IsNullOrWhiteSpace(user.ConfirmPassword))
            {
                ErrorMessage = "Please fill all required fields.";
                return BadRequest(ErrorMessage);
            }

            if (!_emailValidator.CheckRule(user.EmailAddress))
            {
                ErrorMessage += "Email invalid format.";
                isValid = false;
            }

            if (!_passwordValidator.CheckRule(user.Password))
            {
                ErrorMessage += "*Password invalid format." +
                    " It must contains 8 characters, at least one letter and one digit.";
                isValid = false;
            }

            if (!user.Password.Equals(user.ConfirmPassword))
            {
                ErrorMessage += "*Password mismatch.";
                isValid = false;
            }

            if (!isValid)
                return BadRequest(ErrorMessage);

            var (passwordHash256, salt) = _securePassword.EncryptPassword(user.Password);

            try
            {
                var newUser = new User()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailAddress = user.EmailAddress,
                    PasswordHash = Convert.ToBase64String(passwordHash256),
                    PasswordSalt = Convert.ToBase64String(salt)
                };
                await _userService.CreateAccountAsync(newUser);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return BadRequest(ErrorMessage);
            }

            return Ok("Account created.");
        }
    }
}