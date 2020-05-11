﻿using System;
using System.Threading.Tasks;
using Backend_Dis_App.BaseClasses;
using Backend_Dis_App.Database;
using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;
using Backend_Dis_App.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        

        private string ErrorMessage { get; set; }

        public UsersController(IUserService userService, IEmailValidator emailValidator, IPasswordValidator passwordValidator, TaxAppContext db)
        {
            _userService = userService;
            _emailValidator = emailValidator;
            _passwordValidator = passwordValidator;
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
            var token = await _userService.LoginAsync(user);
            if (token)
                return Ok(token);
            else
                return BadRequest("Invalid credentials. Try again");
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }   
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel user)
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(user.EmailAddress) || string.IsNullOrWhiteSpace(user.OldPassword) ||
                string.IsNullOrWhiteSpace(user.NewPassword) || string.IsNullOrWhiteSpace(user.ConfirmNewPassword))
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
                ErrorMessage += "*Password invalid format. It must contains 8 characters, at least one letter and one digit.";
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
            if (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.EmailAddress) || string.IsNullOrWhiteSpace(user.Password) ||
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
                ErrorMessage += "*Password invalid format. It must contains 8 characters, at least one letter and one digit.";
                isValid = false;
            }

            if (!user.Password.Equals(user.ConfirmPassword))
            {
                ErrorMessage += "*Password mismatch.";
                isValid = false;
            }

            if (!isValid)
                return BadRequest(ErrorMessage);

            try
            {
                var newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailAddress = user.EmailAddress,
                    PasswordHash = user.Password,
                    PasswordSalt = user.ConfirmPassword
                };
                await _userService.CreateAccountAsync(newUser);
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return BadRequest(ErrorMessage);
            }    

            return Ok("Account created.");
        }
    }
}
